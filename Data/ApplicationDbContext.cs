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
    public DbSet<StockInOut> StockInOuts { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<TransactionReference> TransactionReferences { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<ProductAuditTrail> ProductAuditTrails { get; set; }

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
        entity.HasKey(bp => new { bp.BranchId, bp.ProductId });
        entity.Property(bp => bp.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(bp => bp.IsActive).HasDefaultValue(true);

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
                     .OnDelete(DeleteBehavior.Restrict);
      });

      // Configure StockInOut entity
      modelBuilder.Entity<StockInOut>(entity =>
      {
        entity.HasKey(st => st.Id);
        entity.Property(st => st.CreatedAt).HasDefaultValueSql("GETDATE()");
        entity.Property(st => st.IsActive).HasDefaultValue(true);

        entity.HasOne(st => st.Branch)
                  .WithMany(b => b.StockInOuts)
                  .HasForeignKey(st => st.BranchId)
                  .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(st => st.Product)
                  .WithMany(p => p.StockInOuts)
                  .HasForeignKey(st => st.ProductId)
                  .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(st => st.TransactionReference)
                  .WithMany(tr => tr.StockInOuts)
                  .HasForeignKey(st => st.TransactionReferenceId)
                  .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(st => st.TransactionType)
                  .WithMany(tt => tt.StockInOuts)
                  .HasForeignKey(st => st.TransactionTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
      });

      // Apply soft delete query filters
      modelBuilder.Entity<Branch>().HasQueryFilter(b => b.DeletedAt == null);
      modelBuilder.Entity<BranchProduct>().HasQueryFilter(bp => bp.DeletedAt == null);
      modelBuilder.Entity<Employee>().HasQueryFilter(e => e.DeletedAt == null);
      modelBuilder.Entity<EmployeeBranch>().HasQueryFilter(eb => eb.DeletedAt == null);
      modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(ep => ep.DeletedAt == null);
      modelBuilder.Entity<StockInOut>().HasQueryFilter(st => st.DeletedAt == null);

    }
  }
}
