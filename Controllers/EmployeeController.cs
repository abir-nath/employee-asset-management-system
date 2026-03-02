using EmployeeAssetManagementSystem.Data;
using EmployeeAssetManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using EmployeeAssetManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAssetManagementSystem.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Index()
    {
        var employees = await _context.Employees
            .Where(e => !e.IsDeleted)
            .ToListAsync();

        return View(employees);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee created successfully!";

            return RedirectToAction(nameof(Index));
        }

        return View(employee);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);

        if (employee == null || !employee.IsActive)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Employee updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == employee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(employee);
    }

    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Details(int id)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Where(e => e.Id == id && !e.IsDeleted)
            .Select(e => new EmployeeDetailsViewModel
            {
                Id = e.Id,
                FullName = e.FullName,
                Email = e.Email,
                Department = e.Department,
                JoiningDate = e.JoiningDate,
                IsActive = e.IsActive,
                //Employee Assets
                Assets = e.EmployeeAssets
                    .Where(ea => ea.Status != "Returned")
                    .Select(ea => new EmployeeAssetViewModel
                    {
                        AssetName = ea.Asset != null ? ea.Asset.AssetName : "",
                        AssetType = ea.Asset != null ? ea.Asset.AssetType : "",
                        AssignedDate = ea.AssignedDate,
                        ReturnedDate = ea.ReturnedDate,
                        Status = ea.Status
                    })
                    .ToList()
            })
    .FirstOrDefaultAsync();

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

        if (employee == null)
        {
            return NotFound();
        }

        employee.IsDeleted = true;

        await _context.SaveChangesAsync();

        TempData["DeleteMessage"] = "Employee deleted successfully!";

        return RedirectToAction(nameof(Index));
    }
}
