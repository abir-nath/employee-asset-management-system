using EmployeeAssetManagementSystem.Data;
using EmployeeAssetManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalEmployees = await _context.Employees
                .CountAsync(e => e.IsActive && !e.IsDeleted),

            TotalAssets = await _context.Assets
                .CountAsync(a => !a.IsDeleted),

            AssignedAssets = await _context.Assets
                .CountAsync(a => !a.IsDeleted && !a.IsAvailable),

            AvailableAssets = await _context.Assets
                .CountAsync(a => !a.IsDeleted && a.IsAvailable),

            RecentAssignments = await _context.EmployeeAssets
                .Include(ea => ea.Employee)
                .Include(ea => ea.Asset)
                .Where(ea => !ea.Employee.IsDeleted && !ea.Asset.IsDeleted)
                .OrderByDescending(ea => ea.AssignedDate)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}