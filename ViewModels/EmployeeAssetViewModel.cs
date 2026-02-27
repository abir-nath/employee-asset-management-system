namespace EmployeeAssetManagementSystem.ViewModels;

public sealed class EmployeeAssetViewModel
{
    public string AssetName { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;

    public DateTime AssignedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }

    public string Status { get; set; } = string.Empty;
}