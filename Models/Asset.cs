using System.ComponentModel.DataAnnotations;

namespace EmployeeAssetManagementSystem.Models;

public class Asset
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Asset Name is required")]
    [StringLength(100)]
    public string AssetName { get; set; }

    [Required(ErrorMessage = "Serial Number is required")]
    [StringLength(50)]
    public string SerialNumber { get; set; }

    [Required(ErrorMessage = "Asset Type is required")]
    public string AssetType { get; set; }

    [Required(ErrorMessage = "Purchase Date is required")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsDeleted { get; set; } = false;
    public ICollection<EmployeeAsset>? EmployeeAssets { get; set; }
}