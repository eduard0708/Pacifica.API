using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models;

namespace Pacifica.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }

        // Configurations for model building
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                // Ensure EmployeeId is not auto-generated and is required
                entity.Property(e => e.EmployeeId)
                      .IsRequired() // EmployeeId is required
                      .HasMaxLength(128); // Set maximum length for EmployeeId

                // Configure the Employee audit fields and soft delete
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true); // Default value for IsActive
                entity.HasOne(e => e.EmployeeProfile)
                      .WithOne(ep => ep.Employee)
                      .HasForeignKey<Employee>(e => e.EmployeeProfileId); // One-to-one relationship with EmployeeProfile
            });

            // Configure Address entity
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id); // Primary key for Address
                entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(a => a.IsActive).HasDefaultValue(true); // Default value for IsActive

                // One-to-one relationship with EmployeeProfile
                entity.HasOne(a => a.EmployeeProfile)
                      .WithOne(ep => ep.Address)
                      .HasForeignKey<Address>(a => a.EmployeeProfileId); // One-to-one relationship with EmployeeProfile
            });

            // Configure EmployeeProfile entity
            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.HasKey(ep => ep.Id); // Primary key for EmployeeProfile
                entity.Property(ep => ep.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(ep => ep.IsActive).HasDefaultValue(true); // Default value for IsActive

                // One-to-one relationship with Employee
                entity.HasOne(ep => ep.Employee)
                      .WithOne(e => e.EmployeeProfile)
                      .HasForeignKey<EmployeeProfile>(ep => ep.EmployeeId); // One-to-one relationship with Employee

                // One-to-one relationship with Address
                entity.HasOne(ep => ep.Address)
                      .WithOne(a => a.EmployeeProfile)
                      .HasForeignKey<EmployeeProfile>(ep => ep.AddressId); // One-to-one relationship with Address
            });

            // Soft delete logic: apply query filters to exclude soft-deleted entities by default
            modelBuilder.Entity<Address>().HasQueryFilter(a => a.DeletedAt == null); // Filter out soft deleted addresses
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null); // Filter out soft deleted employees
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null); // Filter out soft deleted profiles
        }
    }
}
