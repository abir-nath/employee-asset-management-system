namespace EmployeeAssetManagementSystem.ViewModels;

public sealed class EmployeeDetailsViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public bool IsActive { get; set; }

    public List<EmployeeAssetViewModel> Assets { get; set; } = new();
}
