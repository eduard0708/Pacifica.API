using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models.Reports.F154Report;

namespace Pacifica.API.Data
{
    public static class ModelConfigurationF154Reports
    {
        public static void ApplyConfigurations(ModelBuilder modelBuilder)
        {
            // Configure DailySalesReport
            modelBuilder.Entity<DailySalesReport>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with Branch
                entity.HasOne(x => x.Branch)
                    .WithMany() // Assuming that Branch can have many DailySalesReports
                    .HasForeignKey(x => x.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure other properties
                entity.Property(x => x.Date).IsRequired();
            });

            // Configure CashDenomination
            modelBuilder.Entity<CashDenomination>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with DailySalesReport
                entity.HasOne(x => x.DailySalesReport)
                    .WithMany(x => x.CashDenominations)
                    .HasForeignKey(x => x.DailySalesReportId)
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

                // One-to-many relationship with DailySalesReport
                entity.HasOne(x => x.DailySalesReport)
                    .WithMany(x => x.Checks)
                    .HasForeignKey(x => x.DailySalesReportId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure SalesBreakdown
            modelBuilder.Entity<SalesBreakdown>(entity =>
            {
                // Primary key configuration
                entity.HasKey(x => x.Id);

                // One-to-many relationship with DailySalesReport
                entity.HasOne(x => x.DailySalesReport)
                    .WithMany(x => x.SalesBreakdowns)
                    .HasForeignKey(x => x.DailySalesReportId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}


