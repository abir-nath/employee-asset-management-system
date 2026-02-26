using System.ComponentModel.DataAnnotations;

namespace EmployeeAssetManagementSystem.Models;

public class Employee
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Department { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime JoiningDate { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation property
    public ICollection<EmployeeAsset>? EmployeeAssets { get; set; }
}