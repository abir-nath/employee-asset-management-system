using System.ComponentModel.DataAnnotations;

namespace EmployeeAssetManagementSystem.Models;

public class EmployeeAsset
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public int AssetId { get; set; }
    public Asset? Asset { get; set; }

    [Required]
    public DateTime AssignedDate { get; set; }

    public DateTime? ReturnedDate { get; set; }

    [Required]
    public string Status { get; set; } = "Assigned";
}