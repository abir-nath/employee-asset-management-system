using EmployeeAssetManagementSystem.Data;
using EmployeeAssetManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAssetManagementSystem.Controllers;

public class EmployeeAssetsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeAssetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var assignments = await _context.EmployeeAssets
            .Include(ea => ea.Employee)
            .Include(ea => ea.Asset)
            .Where(ea => !ea.Employee.IsDeleted && !ea.Asset.IsDeleted)
            .ToListAsync();

        return View(assignments);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Employees"] = new SelectList(
            await _context.Employees.Where(e => e.IsActive && !e.IsDeleted).ToListAsync(),
            "Id", "FullName");

        ViewData["Assets"] = new SelectList(
            await _context.Assets.Where(e => e.IsAvailable && !e.IsDeleted).ToListAsync(),
            "Id", "AssetName");

        return View(new EmployeeAsset());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeAsset employeeAsset)
    {
        if (employeeAsset.Status == "Returned" && !employeeAsset.ReturnedDate.HasValue)
        {
            ModelState.AddModelError("ReturnedDate",
                "Returned Date is required when status is Returned.");
        }

        if (employeeAsset.ReturnedDate.HasValue && employeeAsset.ReturnedDate < employeeAsset.AssignedDate)
        {
            ModelState.AddModelError("ReturnedDate",
                "Returned Date cannot be earlier than Assigned Date.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["Employees"] = new SelectList(
                await _context.Employees.Where(e => e.IsActive && !e.IsDeleted).ToListAsync(),
                "Id", "FullName", employeeAsset.EmployeeId);

            ViewData["Assets"] = new SelectList(
                await _context.Assets.Where(e => e.IsAvailable && !e.IsDeleted).ToListAsync(),
                "Id", "AssetName", employeeAsset.AssetId);

            return View(employeeAsset);
        }

        var asset = await _context.Assets.FindAsync(employeeAsset.AssetId);

        if(asset == null || !asset.IsAvailable)
        {
            ModelState.AddModelError("AssetId", "Selected asset is not available");

            ViewData["Employees"] = new SelectList(
                await _context.Employees.Where(e => e.IsActive && !e.IsDeleted).ToListAsync(),
                "Id", "FullName", employeeAsset.EmployeeId);

            ViewData["Assets"] = new SelectList(
                await _context.Assets.Where(e => e.IsAvailable && !e.IsDeleted).ToListAsync(),
                "Id", "AssetName", employeeAsset.AssetId);

            return View(employeeAsset);
        }

        asset.IsAvailable = false;

        _context.EmployeeAssets.Add(employeeAsset);
        await _context.SaveChangesAsync();

        TempData["SucessMessage"] = "Asset assigned successfully1";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employeeAsset = await _context.EmployeeAssets.FindAsync(id);

        if (employeeAsset == null)
            return NotFound();

        ViewData["Employees"] = new SelectList(
                await _context.Employees.Where(e => e.IsActive && !e.IsDeleted).ToListAsync(),
                "Id", "FullName", employeeAsset.EmployeeId);

        ViewData["Assets"] = new SelectList(
            await _context.Assets.Where(a => !a.IsDeleted && (a.IsAvailable || a.Id == employeeAsset.AssetId)).ToListAsync(),
            "Id", "AssetName", employeeAsset.AssetId);

        return View(employeeAsset);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeAsset employeeAsset)
    {
        if (employeeAsset.Status == "Returned" && !employeeAsset.ReturnedDate.HasValue)
        {
            ModelState.AddModelError("ReturnedDate",
                "Returned Date is required when status is Returned.");
        }

        if (employeeAsset.ReturnedDate.HasValue &&
            employeeAsset.ReturnedDate < employeeAsset.AssignedDate)
        {
            ModelState.AddModelError("ReturnedDate",
                "Returned Date cannot be earlier than Assigned Date.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["Employees"] = new SelectList(
                await _context.Employees.Where(e => e.IsActive && !e.IsDeleted).ToListAsync(),
                "Id", "FullName", employeeAsset.EmployeeId);

            ViewData["Assets"] = new SelectList(
                await _context.Assets.Where(e => e.IsAvailable && !e.IsDeleted).ToListAsync(),
                "Id", "AssetName", employeeAsset.AssetId);

            return View(employeeAsset);
        }

        var existing = await _context.EmployeeAssets.FindAsync(employeeAsset.Id);

        if (existing == null)
            return NotFound();

        existing.EmployeeId = employeeAsset.EmployeeId;
        existing.AssetId = employeeAsset.AssetId;
        existing.AssignedDate = employeeAsset.AssignedDate;
        existing.Status = employeeAsset.Status;
        existing.ReturnedDate = employeeAsset.Status == "Returned" ? employeeAsset.ReturnedDate : null;

        if(employeeAsset.Status == "Returned")
        {
            var asset = await _context.Assets.FindAsync(existing.AssetId);
            if (asset != null)
                asset.IsAvailable = true;
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Asset Assignement updated successfully!";
        return RedirectToAction(nameof(Index));
    }
}
