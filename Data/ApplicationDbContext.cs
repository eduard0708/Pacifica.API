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
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EmployeeBranch> EmployeeBranches { get; set; }
        public DbSet<StockTransactionInOut> StockTransactions { get; set; }
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
                // Configure EmployeeId as the primary key
                modelBuilder.Entity<Employee>()
                    .HasKey(e => e.EmployeeId); // Explicitly specify EmployeeId as the primary key

                // Ensure EmployeeId is not auto-generated and is required
                entity.Property(e => e.EmployeeId)
                      .IsRequired() // EmployeeId is required
                      .HasMaxLength(128); // Set maximum length for EmployeeId

                // Configure the Employee audit fields and soft delete
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true); // Default value for IsActive

                // One-to-one relationship with EmployeeProfile
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

            // Configure Branch entity
            modelBuilder.Entity<Branch>(entity =>
            {
                // Configure Branch fields
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(b => b.IsActive).HasDefaultValue(true); // Default value for IsActive

                // Many-to-many relationship with Employee (EmployeeBranch)
                entity.HasMany(b => b.EmployeeBranches)
                      .WithOne(e => e.Branch)
                      .HasForeignKey(e => e.BranchId);

                // Many-to-many relationship with Product (BranchProduct)
                entity.HasMany(b => b.BranchProducts)
                      .WithOne(bp => bp.Branch)
                      .HasForeignKey(bp => bp.BranchId);
            });

            // Configure EmployeeBranch entity
            modelBuilder.Entity<EmployeeBranch>(entity =>
            {
                // Composite Key: EmployeeId and BranchId
                entity.HasKey(e => new { e.EmployeeId, e.BranchId });

                // Configure Employee and Branch relationships
                entity.HasOne(e => e.Employee)
                      .WithMany(emp => emp.EmployeeBranches)
                      .HasForeignKey(e => e.EmployeeId);

                entity.HasOne(e => e.Branch)
                      .WithMany(br => br.EmployeeBranches)
                      .HasForeignKey(e => e.BranchId);

                // Configure EmployeeBranch audit fields
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true); // Default value for IsActive
            });

            // Configure the composite key for BranchProduct
            modelBuilder.Entity<BranchProduct>(entity =>
            {
                entity.HasKey(bp => new { bp.BranchId, bp.ProductId }); // Composite key

                // Configure the relationships (foreign keys)
                entity.HasOne(bp => bp.Branch)
                      .WithMany(b => b.BranchProducts)
                      .HasForeignKey(bp => bp.BranchId)
                      .OnDelete(DeleteBehavior.Cascade); // Use Cascade if needed

                entity.HasOne(bp => bp.Product)
                      .WithMany(p => p.BranchProducts)
                      .HasForeignKey(bp => bp.ProductId)
                      .OnDelete(DeleteBehavior.Cascade); // Use Cascade if needed

                // Optionally, you can configure audit fields here if required
                entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(bp => bp.IsActive).HasDefaultValue(true); // Default IsActive to true
            });
            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Configure Product fields
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.IsActive).HasDefaultValue(true); // Default value for IsActive

                // One-to-many relationship with Category
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId);

                // One-to-many relationship with Supplier
                entity.HasOne(p => p.Supplier)
                      .WithMany(s => s.Products)
                      .HasForeignKey(p => p.SupplierId);

                // Soft delete logic: exclude soft-deleted products
                entity.HasQueryFilter(p => p.DeletedAt == null); // Filter out soft-deleted products
            });

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                // Configure Supplier fields
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsActive).HasDefaultValue(true); // Default value for IsActive
            });

            // Configure TransactionReference and TransactionType entities (if needed)
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

            // Soft delete logic: apply query filters to exclude soft-deleted entities by default
            modelBuilder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null); // Filter out soft-deleted branches
            modelBuilder.Entity<BranchProduct>().HasQueryFilter(bp => bp.DeletedAt == null); // Filter out soft-deleted branch products
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null); // Filter out soft-deleted employees
            modelBuilder.Entity<EmployeeBranch>().HasQueryFilter(eb => eb.DeletedAt == null); // Filter out soft-deleted employee-branch relationships
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null); // Filter out soft-deleted profiles
            modelBuilder.Entity<StockTransactionInOut>().HasQueryFilter(ep => ep.DeletedAt == null); // Filter out soft-deleted profiles

        }

    }
}
