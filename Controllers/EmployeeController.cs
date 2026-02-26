using EmployeeAssetManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAssetManagementSystem.Controllers;

public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> ListEmployees()
    {
        var employees = await _context.Employees
            .Where(e => e.IsActive)
            .ToListAsync();

        return View(employees);
    }
}
