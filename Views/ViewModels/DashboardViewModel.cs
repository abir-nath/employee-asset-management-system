using EmployeeAssetManagementSystem.Models;

namespace EmployeeAssetManagementSystem.Models.ViewModels;

public class DashboardViewModel
{
    public int TotalEmployees { get; set; }
    public int TotalAssets { get; set; }
    public int AssignedAssets { get; set; }
    public int AvailableAssets { get; set; }

    public List<EmployeeAsset> RecentAssignments { get; set; } = new();
}