using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Data
{
    public static class ModelConfigurationF154Reports
    {
        public static void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            // Configure F154SalesReport
            modelBuilder.Entity<F154SalesReport>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with Branch
                entity.HasOne(x => x.Branch)
                    .WithMany() // Assuming that Branch can have many F154SalesReports
                    .HasForeignKey(x => x.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);
                    
                // Configure other properties
                entity.Property(x => x.dateReported).IsRequired();
                // Add a unique index on dateReported to ensure distinct values
                entity.HasIndex(x => x.dateReported)
                    .IsUnique(); // Enforces uniqueness
            });

            // Configure CashDenomination
            modelBuilder.Entity<CashDenomination>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with F154SalesReport
                entity.HasOne(x => x.F154SalesReport)
                    .WithMany(x => x.CashDenominations)
                    .HasForeignKey(x => x.F154SalesReportId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Enum conversion for Denomination
                entity.Property(x => x.Denomination)
                    .HasConversion<int>(); // Enum to int conversion for database storage
            });

            // Configure Check
            modelBuilder.Entity<Check>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with F154SalesReport
                entity.HasOne(x => x.F154SalesReport)
                    .WithMany(x => x.Checks)
                    .HasForeignKey(x => x.F154SalesReportId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure SalesBreakdown
            modelBuilder.Entity<SalesBreakdown>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with F154SalesReport
                entity.HasOne(x => x.F154SalesReport)
                    .WithMany(x => x.SalesBreakdowns)
                    .HasForeignKey(x => x.F154SalesReportId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}


