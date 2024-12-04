using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pacifica.API.Models.GlobalAuditTrails;
using Pacifica.API.Models.Inventory;
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

        // DbSets for entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
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




        // Configurations for model building
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // // Automatically set 'Year' based on InventoryDate in the WeeklyInventory
            // modelBuilder.Entity<WeeklyInventory>()
            //     .Property(w => w.Year)
            //     .HasDefaultValueSql("YEAR(GETDATE())");  // Default current year if not provided.

            // // Configure Discrepancy calculation logic (EF won't do the calculation but can define the column behavior)
            // modelBuilder.Entity<WeeklyInventory>()
            //     .Property(w => w.Discrepancy)
            //     .HasComputedColumnSql("[SystemQuantity] - [ActualQuantity]"); // This can create a computed column in the database

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
                entity.HasKey(e => new { e.Id, e.BranchId });
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.EmployeeBranches)
                    .HasForeignKey(e => e.Id);

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

            // Configure Supplier entity
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(s => s.IsActive).HasDefaultValue(true);
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

            // Configure StockIn, StockOut, StockInReference, and StockOutReference entities
            // StockIn entity configuration
            modelBuilder.Entity<StockIn>()
                .HasKey(si => si.Id);

            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Product)
                .WithMany(p => p.StockIns)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.Branch)
                .WithMany(b => b.StockIns)
                .HasForeignKey(si => si.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockIn>()
                .HasOne(si => si.StockInReference)
                .WithMany(b => b.StockIns)
                .HasForeignKey(si => si.StockInReferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure StockIn entity decimal properties
            modelBuilder.Entity<StockIn>()
                .Property(si => si.CostPrice)
                .HasColumnType("decimal(18,2)"); // Specify precision and scale for CostPrice

            // StockOut entity configuration
            modelBuilder.Entity<StockOut>()
                .HasKey(so => so.Id);

            modelBuilder.Entity<StockOut>()
                .HasOne(so => so.Product)
                .WithMany(p => p.StockOuts)
                .HasForeignKey(so => so.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOut>()
                .HasOne(so => so.Branch)
                .WithMany(b => b.StockOuts)
                .HasForeignKey(so => so.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StockOut>()
                .HasOne(so => so.StockOutReference)
                .WithMany(b => b.StockOuts)
                .HasForeignKey(so => so.StockOutReferenceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure StockOut entity decimal properties
            modelBuilder.Entity<StockOut>()
                .Property(so => so.RetailPrice)
                .HasColumnType("decimal(18,2)"); // Specify precision and scale for RetailPrice


            // Configure BranchProduct to BeginningInventory relationship
            modelBuilder.Entity<WeeklyInventory>()
                .HasOne(bi => bi.BranchProduct)
                .WithMany(bp => bp.WeeklyInventories)
                .HasForeignKey(bi => new { bi.BranchId, bi.ProductId }) // Composite FK
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WeeklyInventory>()
                .Property(w => w.SystemQuantity)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<WeeklyInventory>()
                .Property(w => w.ActualQuantity)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<WeeklyInventory>()
                .Property(w => w.Discrepancy)
                .HasColumnType("decimal(18, 2)");

            // // Configure BranchProduct to MonthlyInventory relationship
            // modelBuilder.Entity<MonthlyInventory>()
            //     .HasOne(mi => mi.BranchProduct)
            //     .WithMany(bp => bp.MonthlyInventories)
            //     .HasForeignKey(mi => new { mi.BranchId, mi.ProductId }) // Composite FK
            //     .OnDelete(DeleteBehavior.Restrict);

            // // Configure BranchProduct to AuditTrail relationship
            // modelBuilder.Entity<BranchProductInventoryAuditTrail>()
            //     .HasOne(bait => bait.BranchProduct)
            //     .WithMany(bp => bp.BranchProductInventoryAuditTrails)
            //     .HasForeignKey(bait => new { bait.BranchId, bait.ProductId }) // Composite FK
            //     .OnDelete(DeleteBehavior.Restrict);

            // // Optionally, configure other properties like indexes or default values
            // modelBuilder.Entity<BranchProductInventoryAuditTrail>()
            //     .HasIndex(bait => new { bait.BranchId, bait.ProductId, bait.DateAdjusted })
            //     .HasDatabaseName("IX_AuditTrail_BranchProduct_Date");

            // modelBuilder.Entity<MonthlyInventory>()
            //     .HasIndex(mi => new { mi.BranchId, mi.ProductId, mi.Date })
            //     .HasDatabaseName("IX_MonthlyInventory_BranchProduct_Date");



            // Apply soft delete query filters
            modelBuilder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null);
            modelBuilder.Entity<BranchProduct>().HasQueryFilter(bp => bp.DeletedAt == null);
            modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null);
            modelBuilder.Entity<EmployeeBranch>().HasQueryFilter(eb => eb.DeletedAt == null);
            modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null);
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


        }
    }
}
