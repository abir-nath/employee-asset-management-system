namespace EmployeeAssetManagementSystem.ViewModels;

public sealed class AssetEmployeeViewModel
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime AssignedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
