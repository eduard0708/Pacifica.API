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
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EmployeeBranch> EmployeeBranches { get; set; }
        public DbSet<StockTransactionInOut> stockTransactionInOuts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<TransactionReference> TransactionReferences { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        // Configurations for model building
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.EmployeeProfile)
                      .WithOne(ep => ep.Employee)
                      .HasForeignKey<Employee>(e => e.EmployeeProfileId);
            });

            // Configure Address entity
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(a => a.IsActive).HasDefaultValue(true);

                entity.HasOne(a => a.EmployeeProfile)
                      .WithOne(ep => ep.Address)
                      .HasForeignKey<Address>(a => a.EmployeeProfileId);
            });

            // Configure EmployeeProfile entity
            modelBuilder.Entity<EmployeeProfile>(entity =>
            {
                entity.HasKey(ep => ep.Id);
                entity.Property(ep => ep.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(ep => ep.IsActive).HasDefaultValue(true);

                entity.HasOne(ep => ep.Employee)
                      .WithOne(e => e.EmployeeProfile)
                      .HasForeignKey<EmployeeProfile>(ep => ep.EmployeeId);

                entity.HasOne(ep => ep.Address)
                      .WithOne(a => a.EmployeeProfile)
                      .HasForeignKey<EmployeeProfile>(ep => ep.AddressId);
            });

            // Configure Branch entity
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(b => b.IsActive).HasDefaultValue(true);

                entity.HasMany(b => b.EmployeeBranches)
                      .WithOne(e => e.Branch)
                      .HasForeignKey(e => e.BranchId);

                entity.HasMany(b => b.BranchProducts)
                      .WithOne(bp => bp.Branch)
                      .HasForeignKey(bp => bp.BranchId);
            });

            // Configure EmployeeBranch entity
            modelBuilder.Entity<EmployeeBranch>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeId, e.BranchId });
                entity.HasOne(e => e.Employee)
                      .WithMany(emp => emp.EmployeeBranches)
                      .HasForeignKey(e => e.EmployeeId);

                entity.HasOne(e => e.Branch)
                      .WithMany(br => br.EmployeeBranches)
                      .HasForeignKey(e => e.BranchId);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Configure BranchProduct entity
            modelBuilder.Entity<BranchProduct>(entity =>
            {
                entity.HasKey(bp => new { bp.BranchId, bp.ProductId });

                entity.HasOne(bp => bp.Branch)
                      .WithMany(b => b.BranchProducts)
                      .HasForeignKey(bp => bp.BranchId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bp => bp.Product)
                      .WithMany(p => p.BranchProducts)
                      .HasForeignKey(bp => bp.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(bp => bp.IsActive).HasDefaultValue(true);
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.IsActive).HasDefaultValue(true);

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId);

                entity.HasOne(p => p.Supplier)
                      .WithMany(s => s.Products)
                      .HasForeignKey(p => p.SupplierId);

                entity.HasQueryFilter(p => p.DeletedAt == null);
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsActive).HasDefaultValue(true);
            });

            // Configure TransactionReference and TransactionType entities
            modelBuilder.Entity<TransactionReference>(entity =>
            {
                entity.Property(tr => tr.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(tr => tr.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(tt => tt.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(tt => tt.IsActive).HasDefaultValue(true);
            });

            // Configure StockTransactionInOut entity
            modelBuilder.Entity<StockTransactionInOut>(entity =>
            {
                // Primary Key
                entity.HasKey(st => st.Id);

                // Foreign Key configurations
                entity.HasOne(st => st.Product)
                      .WithMany()
                      .HasForeignKey(st => st.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(st => st.TransactionType)
                      .WithMany()
                      .HasForeignKey(st => st.TransactionTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.TransactionReference)
                      .WithMany()
                      .HasForeignKey(st => st.TransactionReferenceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(st => st.Branch)
                      .WithMany()
                      .HasForeignKey(st => st.BranchId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configure audit fields
                entity.Property(st => st.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(st => st.IsActive).HasDefaultValue(true);
            });

            // Soft delete logic: apply query filters to exclude soft-deleted entities
            modelBuilder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null);
            modelBuilder.Entity<BranchProduct>().HasQueryFilter(bp => bp.DeletedAt == null);
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<EmployeeBranch>().HasQueryFilter(eb => eb.DeletedAt == null);
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null);
            modelBuilder.Entity<StockTransactionInOut>().HasQueryFilter(st => st.DeletedAt == null); // Exclude soft-deleted transactions
        }
    }
}
