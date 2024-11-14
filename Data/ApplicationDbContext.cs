using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            public DbSet<StockTransactionInOut> StockTransactionInOuts { get; set; }
            public DbSet<Supplier> Suppliers { get; set; }
            public DbSet<TransactionReference> TransactionReferences { get; set; }
            public DbSet<TransactionType> TransactionTypes { get; set; }

            public DbSet<ProductStatus> ProductStatuses { get; set; }

            //   public object StockTransactions { get; internal set; }

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

                  // Configure Address entity (explicitly create the table here)
                  modelBuilder.Entity<Address>(entity =>
                  {
                        entity.HasKey(a => a.Id); // Define the primary key
                        entity.Property(a => a.CreatedAt).HasDefaultValueSql("GETDATE()");
                        entity.Property(a => a.IsActive).HasDefaultValue(true);

                        // Define the relationship to EmployeeProfile
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
                        entity.HasKey(e => new { e.Id, e.BranchId });
                        entity.HasOne(e => e.Employee)
                        .WithMany(emp => emp.EmployeeBranches)
                        .HasForeignKey(e => e.Id);

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

                        
                        entity.HasOne(bp => bp.ProductStatus)
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

                  // Configure ProductStatus  entities
                  modelBuilder.Entity<ProductStatus>(entity =>
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

                        // Configure foreign keys explicitly for each relationship
                        entity.HasOne(st => st.Branch)
                        .WithMany(b => b.StockTransactionInOuts)
                        .HasForeignKey(st => st.BranchId)
                        .OnDelete(DeleteBehavior.Cascade); // Add delete behavior as needed

                        entity.HasOne(st => st.Product)
                        .WithMany(b => b.StockTransactionInOuts)
                        .HasForeignKey(st => st.ProductId)
                        .OnDelete(DeleteBehavior.Cascade); // Add delete behavior as needed

                        entity.HasOne(st => st.TransactionReference)
                        .WithMany(b => b.StockTransactionInOuts)
                        .HasForeignKey(st => st.TransactionReferenceId)
                        .OnDelete(DeleteBehavior.Restrict); // Add delete behavior as needed

                        entity.HasOne(st => st.TransactionType)
                        .WithMany(b => b.StockTransactionInOuts)
                        .HasForeignKey(st => st.TransactionTypeId)
                        .OnDelete(DeleteBehavior.Restrict); // Add delete behavior as needed

                        // Configure common audit fields
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

                  // Seed data for TransactionReference
                  modelBuilder.Entity<TransactionReference>().HasData(
                      new TransactionReference { Id = 1, TransactionReferenceName = "Supplier Delivery (BMEG)" },
                      new TransactionReference { Id = 2, TransactionReferenceName = "Branch Sales Transaction" },
                      new TransactionReference { Id = 3, TransactionReferenceName = "Branch Transfer-In" },
                      new TransactionReference { Id = 4, TransactionReferenceName = "Branch Transfer-Out" }
                  );

                  // Seed data for Branch
                  modelBuilder.Entity<Branch>().HasData(
                      new Branch { Id = 1, BranchName = "Roxas Center" },
                      new Branch { Id = 2, BranchName = "Kalibo Toting Reyes" },
                      new Branch { Id = 3, BranchName = "Iloilo Valeria" },
                      new Branch { Id = 4, BranchName = "Antique" },
                      new Branch { Id = 5, BranchName = "Iloilo Super Market" }
                  );

                  // Seed data for Category
                  modelBuilder.Entity<Category>().HasData(
                      new Category { Id = 1, CategoryName = "Fish Foods" },
                      new Category { Id = 2, CategoryName = "Aquarium Accessories" },
                      new Category { Id = 3, CategoryName = "Hog Feeds" },
                      new Category { Id = 4, CategoryName = "Chicken Feeds" },
                      new Category { Id = 5, CategoryName = "Bird Feeds" }
                  );

                  // Seed data for Supplier
                  modelBuilder.Entity<Supplier>().HasData(
                      new Supplier { Id = 1, SupplierName = "AKFF AKWARYUM PETS" },
                      new Supplier { Id = 2, SupplierName = "AQUA GOLD TRADING/AQUATINUM CORP" },
                      new Supplier { Id = 3, SupplierName = "ASVET INC." },
                      new Supplier { Id = 4, SupplierName = "BELMAN LABORATORIES" },
                      new Supplier { Id = 5, SupplierName = "GENERAL ANIMAL FEED & NUTRITION" }
                  );

                   // Seed data for ProductStatus
                  modelBuilder.Entity<ProductStatus>().HasData(
                      new ProductStatus { Id = 1, ProductStatusName = "Available" },
                      new ProductStatus { Id = 2, ProductStatusName = "OutOfStock" }
                  );

                  // Seed data for TransactionType
                  modelBuilder.Entity<TransactionType>().HasData(
                      new TransactionType { Id = 1, TransactionTypeName = "Transaction Stock-In" },
                      new TransactionType { Id = 2, TransactionTypeName = "Transaction Stock-Out" }
                  );


                  // Seed data for Product
                  modelBuilder.Entity<Product>().HasData(
                      new Product
                      {
                            Id = 1,
                            ProductName = "Fish Food A",
                            SKU = "SKU001",
                            ProductStatus = "Available",
                            CategoryId = 1,  // Linking to "Fish Foods"
                            SupplierId = 1,  // Linking to "AKFF AKWARYUM PETS"
                            IsActive = true
                      },
                      new Product
                      {
                            Id = 2,
                            ProductName = "Aquarium Filter",
                            SKU = "SKU002",
                            ProductStatus = "OutOfStock",
                            CategoryId = 2,  // Linking to "Aquarium Accessories"
                            SupplierId = 2,  // Linking to "AQUA GOLD TRADING/AQUATINUM CORP"
                            IsActive = true
                      },
                      new Product
                      {
                            Id = 3,
                            ProductName = "Hog Feed B",
                            SKU = "SKU003",
                            ProductStatus = "Available",
                            CategoryId = 3,  // Linking to "Hog Feeds"
                            SupplierId = 3,  // Linking to "ASVET INC."
                            IsActive = true
                      },
                      new Product
                      {
                            Id = 4,
                            ProductName = "Chicken Feed C",
                            SKU = "SKU004",
                            ProductStatus = "Available",
                            CategoryId = 4,  // Linking to "Chicken Feeds"
                            SupplierId = 4,  // Linking to "BELMAN LABORATORIES"
                            IsActive = true
                      },
                      new Product
                      {
                            Id = 5,
                            ProductName = "Bird Feed D",
                            SKU = "SKU005",
                            ProductStatus = "OutOfStock",
                            CategoryId = 5,  // Linking to "Bird Feeds"
                            SupplierId = 5,  // Linking to "GENERAL ANIMAL FEED & NUTRITION"
                            IsActive = false
                      }
                  );

            }
      }
}
