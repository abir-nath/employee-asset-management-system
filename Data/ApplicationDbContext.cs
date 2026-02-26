using Microsoft.EntityFrameworkCore;
using EmployeeAssetManagementSystem.Models;

namespace EmployeeAssetManagementSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<EmployeeAsset> EmployeeAssets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeAsset>()
            .HasOne(ea => ea.Employee)
            .WithMany(e => e.EmployeeAssets)
            .HasForeignKey(ea => ea.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeAsset>()
            .HasOne(ea => ea.Asset)
            .WithMany(a => a.EmployeeAssets)
            .HasForeignKey(ea => ea.AssetId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}