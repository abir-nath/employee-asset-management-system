using EmployeeAssetManagementSystem.Data;
using EmployeeAssetManagementSystem.Models;
using EmployeeAssetManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAssetManagementSystem.Controllers;

[Authorize]
public class AssetsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Index()
    {
        var assets = await _context.Assets
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        return View(assets);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Asset asset)
    {
        if (!ModelState.IsValid)
        {
            ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });
            return View(asset);
        }
        var serialExists = await _context.Assets
            .AnyAsync(a => a.SerialNumber == asset.SerialNumber && !a.IsDeleted);

        if (serialExists)
        {
            ModelState.AddModelError("SerialNumber", "Serial Number already exists.");
            ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });
            return View(asset);
        }

        asset.IsAvailable = true;

        _context.Add(asset);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Asset created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var asset = await _context.Assets.FindAsync(id);

        if (asset == null || asset.IsDeleted)
        {
            return NotFound();
        }

        ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });

        return View(asset);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Asset asset)
    {
        if (id != asset.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });
            return View(asset);
        }
        var serialExists = await _context.Assets
            .AnyAsync(a => a.SerialNumber == asset.SerialNumber
                        && a.Id != asset.Id
                        && !a.IsDeleted);

        if (serialExists)
        {
            ModelState.AddModelError("SerialNumber", "Serial Number already exists.");
            ViewData["AssetTypes"] = new SelectList(new[] { "Monitor", "Laptop", "Keyboard", "Mouse", "Headset" });
            return View(asset);
        }

        _context.Update(asset);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Asset updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Details(int id)
    {
        var asset = await _context.Assets
        .AsNoTracking()
        .Where(a => a.Id == id && !a.IsDeleted)
        .Select(a => new AssetDetailsViewModel
        {
            Id = a.Id,
            AssetName = a.AssetName,
            SerialNumber = a.SerialNumber,
            AssetType = a.AssetType,
            PurchaseDate = a.PurchaseDate,
            IsAvailable = a.IsAvailable,
            EmployeeAssets = a.EmployeeAssets
                .Select(ea => new AssetEmployeeViewModel
                {
                    EmployeeId = ea.EmployeeId,
                    EmployeeName = ea.Employee.FullName,
                    AssignedDate = ea.AssignedDate,
                    ReturnedDate = ea.ReturnedDate,
                    Status = ea.Status
                })
                .ToList()
        })
        .FirstOrDefaultAsync();

        if (asset == null)
        {
            return NotFound();
        }

        return View(asset);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (asset == null)
        {
            return NotFound();
        }

        asset.IsDeleted = true;

        await _context.SaveChangesAsync();

        TempData["DeleteMessage"] = "Asset deleted successfully!";

        return RedirectToAction(nameof(Index));
    }
}
