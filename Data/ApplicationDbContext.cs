using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pacifica.API.Models.EmployeManagement;
using Pacifica.API.Models.GlobalAuditTrails;
using Pacifica.API.Models.Inventory;
using Pacifica.API.Models.Reports.F154Report;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignore the PendingModelChangesWarning
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<BranchProduct> BranchProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EmployeeBranch> EmployeeBranches { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<StockInReference> StockInReferences { get; set; }
        public DbSet<StockOutReference> StockOutReferences { get; set; }
        public DbSet<StockOut> StockOuts { get; set; }
        public DbSet<StockIn> StockIns { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<BranchProductAuditTrail> BranchProductAuditTrails { get; set; }
        public DbSet<ProductAuditTrail> ProductAuditTrails { get; set; }
        public DbSet<StockInAuditTrail> StockInAuditTrails { get; set; }
        public DbSet<StockOutAuditTrail> StockOutAuditTrails { get; set; }
        public DbSet<WeeklyInventory> WeeklyInventories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryNormalization> InventoryNormalizations { get; set; }


        /// <summary>
        ///  Imported form ModelConfigurationF154Reports Class the configuration
        /// </summary>
        public DbSet<F154SalesReport> F154SalesReports { get; set; }
        public DbSet<CashDenomination> CashDenominations { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<SalesBreakdown> SalesBreakdowns { get; set; }
        public DbSet<Less> Lesses { get; set; }
        public DbSet<InclusiveInvoiceType> InclusiveInvoiceTypes { get; set; }


        // Configurations for model building
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
                {

                    // Configure EmployeeId as a unique index
                    entity.HasIndex(e => e.EmployeeId)
                          .IsUnique();

                    entity.Property(e => e.EmployeeId)
                            .IsRequired()
                            .HasMaxLength(25); // Example constraint

                    entity.Property(e => e.FirstName)
                        .HasMaxLength(20)
                        .IsRequired(false); // Optional field

                    entity.Property(e => e.LastName)
                        .HasMaxLength(20)
                        .IsRequired(false); // Optional field

                    entity.Property(e => e.CreatedAt)
                          .IsRequired()
                          .HasDefaultValueSql("GETDATE()");

                    // Configure the one-to-one relationship with EmployeeProfile
                    entity.HasOne(e => e.EmployeeProfile)
                          .WithOne(ep => ep.Employee)
                          .HasForeignKey<EmployeeProfile>(ep => ep.EmployeeId)
                          .OnDelete(DeleteBehavior.Restrict);  // If Employee is deleted, delete EmployeeProfile as well
                });

            // Configure EmployeeProfile entity
            modelBuilder.Entity<EmployeeProfile>(entity =>
                      {
                          entity.HasKey(ep => ep.Id);

                          // Properties
                          entity.Property(ep => ep.EmployeeId)
                            .IsRequired()
                            .HasMaxLength(450);

                          entity.Property(ep => ep.Region)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.Province)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.CityOrMunicipality)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.Barangay)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.StreetAddress)
                  .HasMaxLength(255);

                          entity.Property(ep => ep.PostalCode)
                  .HasMaxLength(10);

                          entity.Property(ep => ep.Country)
                  .HasMaxLength(50)
                  .HasDefaultValue("Philippines");

                          entity.Property(ep => ep.CreatedAt)
                  .IsRequired()
                  .HasDefaultValueSql("GETDATE()");

                          entity.Property(ep => ep.CreatedBy)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.UpdatedAt)
                  .IsRequired(false);

                          entity.Property(ep => ep.DeletedAt)
                  .IsRequired(false);

                          entity.Property(ep => ep.UpdatedBy)
                  .HasMaxLength(100);

                          entity.Property(ep => ep.DeletedBy)
                  .HasMaxLength(100);

                          // Configure the relationship with Employee
                          entity.HasOne(ep => ep.Employee)
                  .WithOne(e => e.EmployeeProfile)
                  .HasForeignKey<EmployeeProfile>(ep => ep.EmployeeId);
                      });

            // Configure Branch entity
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(b => b.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(b => b.IsDeleted).HasDefaultValue(false);

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
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.EmployeeBranches)
                    .HasForeignKey(e => e.EmployeeId);

                entity.HasOne(e => e.Branch)
                    .WithMany(br => br.EmployeeBranches)
                    .HasForeignKey(e => e.BranchId);
            });

            // Configure BranchProduct entity
            modelBuilder.Entity<BranchProduct>(entity =>
            {
                entity.HasKey(bp => new { bp.BranchId, bp.ProductId }); // Composite primary key
                entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()");

                entity.HasOne(bp => bp.Branch)
                    .WithMany(b => b.BranchProducts)
                    .HasForeignKey(bp => bp.BranchId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bp => bp.Product)
                    .WithMany(p => p.BranchProducts)
                    .HasForeignKey(bp => bp.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bp => bp.Status)
                    .WithMany(p => p.BranchProducts)
                    .HasForeignKey(bp => bp.StatusId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Link to the Audit Trails
                entity.HasMany(bp => bp.BranchProductAuditTrails)
                    .WithOne(bpat => bpat.BranchProduct)
                    .HasForeignKey(bpat => new { bpat.BranchId, bpat.ProductId })  // Adjusted to match the composite key
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Configure BranchProductAuditTrail entity
            modelBuilder.Entity<BranchProductAuditTrail>(entity =>
            {
                entity.ToTable("BranchProductAuditTrails");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ActionBy).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ActionDate).IsRequired();
                entity.Property(e => e.OldValue).HasColumnType("text");
                entity.Property(e => e.NewValue).HasColumnType("text");

                entity.HasOne(bpat => bpat.BranchProduct)
                    .WithMany(bp => bp.BranchProductAuditTrails)
                    .HasForeignKey(bpat => new { bpat.BranchId, bpat.ProductId }) // Use both BranchId and ProductId as foreign key
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(); // Make the relationship optional
            });

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasQueryFilter(p => p.DeletedAt == null);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Products)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsDeleted).HasDefaultValue(false);

            });

            // InventoryNormalization entity configuration
            modelBuilder.Entity<InventoryNormalization>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsDeleted).HasDefaultValue(false);
                entity.ToTable("InventoryNormalizations");

                entity.Property(n => n.AdjustedQuantity)
                      .HasColumnType("decimal(18, 2)");

                // Foreign key configuration (optional; inferred by default)
                entity.HasOne(n => n.Inventory)
                      .WithMany(i => i.Normalizations)
                      .HasForeignKey(n => n.InventoryId)
                      .OnDelete(DeleteBehavior.Restrict); // Ensure cascade delete
            });


            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsDeleted).HasDefaultValue(false);

            });

            // Configure ProductAuditTrail
            modelBuilder.Entity<ProductAuditTrail>(entity =>
            {
                entity.ToTable("ProductAuditTrails");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ActionBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ActionDate).IsRequired();
                entity.Property(e => e.OldValue).HasColumnType("text");
                entity.Property(e => e.NewValue).HasColumnType("text");

                entity.HasOne(pa => pa.Product)  // Link to Product
                    .WithMany(p => p.ProductAuditTrails)  // A Product can have many ProductAuditTrailProducts
                    .HasForeignKey(patp => patp.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(); // Make the relationship optional
            });

            // Configure StockInAuditTrail
            modelBuilder.Entity<StockInAuditTrail>(entity =>
            {
                entity.ToTable("StockInAuditTrails");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ActionBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ActionDate).IsRequired();
                entity.Property(e => e.OldValue).HasColumnType("text");
                entity.Property(e => e.NewValue).HasColumnType("text");

                entity.HasOne(si => si.StockIn)  // Link to Product
                    .WithMany(s => s.StockInAuditTrails)  // A Product can have many ProductAuditTrailProducts
                    .HasForeignKey(siat => siat.StockInId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(); // Make the relationship optional
            });

            // Configure StockOuAuditTrail
            modelBuilder.Entity<StockOutAuditTrail>(entity =>
            {
                entity.ToTable("StockOuAuditTrails");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ActionBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ActionDate).IsRequired();
                entity.Property(e => e.OldValue).HasColumnType("text");
                entity.Property(e => e.NewValue).HasColumnType("text");

                entity.HasOne(si => si.StockOut)  // Link to Product
                    .WithMany(s => s.StockOutAuditTrails)  // A Product can have many ProductAuditTrailProducts
                    .HasForeignKey(soit => soit.StockOutId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(); // Make the relationship optional
            });

            // StockIn entity configuration
            modelBuilder.Entity<StockIn>(entity =>
            {
                // Primary Key
                entity.HasKey(si => si.Id);

                // Relationships
                entity.HasOne(si => si.Product)
                      .WithMany(p => p.StockIns)
                      .HasForeignKey(si => si.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(si => si.Branch)
                      .WithMany(b => b.StockIns)
                      .HasForeignKey(si => si.BranchId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(si => si.StockInReference)
                      .WithMany(b => b.StockIns)
                      .HasForeignKey(si => si.StockInReferenceId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal properties
                entity.Property(e => e.CostPrice).HasColumnType("decimal(18,4)");
                entity.Property(e => e.RetailPrice).HasColumnType("decimal(18,4)");
            });

            // StockOut entity configuration
            modelBuilder.Entity<StockOut>(entity =>
            {
                // Primary Key
                entity.HasKey(so => so.Id);

                // Relationships
                entity.HasOne(so => so.Product)
                      .WithMany(p => p.StockOuts)
                      .HasForeignKey(so => so.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(so => so.Branch)
                      .WithMany(b => b.StockOuts)
                      .HasForeignKey(so => so.BranchId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(so => so.StockOutReference)
                      .WithMany(b => b.StockOuts)
                      .HasForeignKey(so => so.StockOutReferenceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(so => so.PaymentMethod)
                      .WithMany(b => b.StockOuts)
                      .HasForeignKey(so => so.PaymentMethodId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal properties
                entity.Property(so => so.RetailPrice)
                      .HasColumnType("decimal(18,2)"); // Specify precision and scale for RetailPrice
            });

            // Configure WeeklyInventory entity
            modelBuilder.Entity<WeeklyInventory>(entity =>
            {
                // Configure composite foreign key relationship with BranchProduct
                entity.HasOne(wi => wi.BranchProduct)
                      .WithMany(bp => bp.WeeklyInventories)
                      .HasForeignKey(wi => new { wi.BranchId, wi.ProductId }) // Composite FK
                      .OnDelete(DeleteBehavior.Restrict);

                // Configure decimal properties for WeeklyInventory
                entity.Property(wi => wi.SystemQuantity)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(wi => wi.ActualQuantity)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(wi => wi.Discrepancy)
                      .HasColumnType("decimal(18, 2)");

                // Set default values for CreatedAt and InventoryDate
                entity.Property(wi => wi.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // Inventory entity configuration
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventories");
                entity.HasKey(i => i.Id);

                // Configure composite foreign key relationship with BranchProduct
                entity.HasOne(wi => wi.BranchProduct)
                      .WithMany(bp => bp.Inventories)
                      .HasForeignKey(wi => new { wi.BranchId, wi.ProductId }) // Composite FK
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(i => i.ActualQuantity)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(i => i.CostPrice)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(i => i.SystemQuantity)
                      .HasColumnType("decimal(8, 2)");

                entity.Property(i => i.Discrepancy)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(i => i.DiscrepancyValue)
                      .HasColumnType("decimal(18, 2)");

                // Define one-to-many relationship with InventoryNormalization
                entity.HasMany(i => i.Normalizations)
                      .WithOne(n => n.Inventory)
                      .HasForeignKey(n => n.InventoryId)
                      .OnDelete(DeleteBehavior.Restrict); // Cascade delete normalizations when inventory is deleted
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                // Configure Index for Department Name to ensure it's unique
                entity.HasIndex(d => d.Name)
                      .IsUnique();

                // Configure optional properties or other settings
                entity.Property(d => d.Name)
                      .HasMaxLength(255) // Assuming you want a maximum length for the name
                      .IsRequired(); // Mark it as required

                // Set default value for CreatedAt
                entity.Property(d => d.DeletedAt)
                      .HasDefaultValue(null); // Default to null unless marked deleted
            });

            // Configure Position entity
            modelBuilder.Entity<Position>(entity =>
            {
                // Configure Index for Position Name to ensure it's unique
                entity.HasIndex(p => p.Name)
                      .IsUnique();

                // Configure optional properties or other settings
                entity.Property(p => p.Name)
                      .HasMaxLength(255) // Assuming you want a maximum length for the name
                      .IsRequired(); // Mark it as required

                // Set default value for CreatedAt
                entity.Property(p => p.DeletedAt)
                      .HasDefaultValue(null); // Default to null unless marked deleted
            });

            // Configure Department-Employee relationship (1-to-many)
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Department if Employee exists

            });

            // Configure Department-Employee relationship (1-to-many)
            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Position)
                    .HasForeignKey(e => e.PositionId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Position if Employee exists


            });


            // Apply soft delete query filters
            modelBuilder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null);
            modelBuilder.Entity<BranchProduct>().HasQueryFilter(bp => bp.DeletedAt == null);
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<EmployeeBranch>().HasQueryFilter(eb => eb.DeletedAt == null);
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null);
            modelBuilder.Entity<PaymentMethod>().HasQueryFilter(ep => ep.DeletedAt == null);
            modelBuilder.Entity<ProductAuditTrail>().HasQueryFilter(b => b.Product != null && b.Product.DeletedAt == null);
            modelBuilder.Entity<BranchProductAuditTrail>().HasQueryFilter(b => b.BranchProduct != null && b.BranchProduct.DeletedAt == null);
            modelBuilder.Entity<StockIn>()
            .HasQueryFilter(si => si.Branch!.DeletedAt == null); // Filter on StockIn

            modelBuilder.Entity<StockOut>()
            .HasQueryFilter(si => si.Branch!.DeletedAt == null); // Filter on StockOut

            // Disable soft delete query filters on StockInAuditTrail to ensure audit data is always visible
            modelBuilder.Entity<StockInAuditTrail>()
                .HasQueryFilter(siat => siat == null); // This can be adjusted if there's a "DeletedAt" field on the audit itself.

            // Disable soft delete query filters on StockInAuditTrail to ensure audit data is always visible
            modelBuilder.Entity<StockOutAuditTrail>()
                .HasQueryFilter(siat => siat == null); // This can be adjusted if there's a "DeletedAt" field on the audit itself.

            // Ensure that global query filters on StockIn and Branch do not interfere with the audit trail
            modelBuilder.Entity<StockIn>()
                .HasQueryFilter(si => si.DeletedAt == null || si.DeletedAt != null);  // Adjust depending on soft-delete behavior

            // Ensure that global query filters on StockIn and Branch do not interfere with the audit trail
            modelBuilder.Entity<StockOut>()
                .HasQueryFilter(si => si.DeletedAt == null || si.DeletedAt != null);  // Adjust depending on soft-delete behavior

            // Ensure that global query filters on StockIn and Branch do not interfere with the audit trail
            modelBuilder.Entity<WeeklyInventory>()
                .HasQueryFilter(si => si.DeletedAt == null || si.DeletedAt != null);  // Adjust depending on soft-delete behavior

            // Ensure that global query filters on StockIn and Branch do not interfere with the audit trail
            modelBuilder.Entity<Inventory>()
                .HasQueryFilter(si => si.DeletedAt == null || si.DeletedAt != null);  // Adjust depending on soft-delete behavior
            modelBuilder.Entity<InventoryNormalization>()
                .HasQueryFilter(si => si.DeletedAt == null || si.DeletedAt != null);  // Adjust depending on soft-delete behavior    

            modelBuilder.Entity<F154SalesReport>().HasQueryFilter(dsr => dsr.DeletedAt == null);
            modelBuilder.Entity<SalesBreakdown>()
                .HasQueryFilter(sb => sb.F154SalesReport!.DeletedAt == null);  // Apply matching filter for SalesBreakdown
            modelBuilder.Entity<CashDenomination>()
                .HasQueryFilter(sb => sb.F154SalesReport!.DeletedAt == null);  // Apply matching filter for SalesBreakdown    
            modelBuilder.Entity<Check>()
                .HasQueryFilter(sb => sb.F154SalesReport!.DeletedAt == null);  // Apply matching filter for SalesBreakdown        
            modelBuilder.Entity<Less>()
                .HasQueryFilter(sb => sb.F154SalesReport!.DeletedAt == null);  // Apply matching filter for SalesBreakdown        
            modelBuilder.Entity<InclusiveInvoiceType>()
          .HasQueryFilter(sb => sb.F154SalesReport!.DeletedAt == null);  // Apply matching filter for SalesBreakdown             


        }
    }
}
