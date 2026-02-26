using System.ComponentModel.DataAnnotations;

namespace EmployeeAssetManagementSystem.Models;

public class Asset
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string AssetName { get; set; }

    [Required]
    public string AssetType { get; set; }

    [Required]
    public string SerialNumber { get; set; }

    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Navigation property
    public ICollection<EmployeeAsset>? EmployeeAssets { get; set; }
}