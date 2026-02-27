namespace EmployeeAssetManagementSystem.ViewModels;

public sealed class AssetDetailsViewModel
{
    public int Id { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public bool IsAvailable { get; set; }

    public List<AssetEmployeeViewModel> EmployeeAssets { get; set; } = new();
}
