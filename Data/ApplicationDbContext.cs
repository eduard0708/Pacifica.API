using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pacifica.API.Models;

namespace Pacifica.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // DbSet for UserProfile
        public DbSet<UserProfile> UserProfiles { get; set; }

        // Configure relationships
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure UserProfile with EmployeeId as primary key (non-auto-increment)
            builder.Entity<UserProfile>()
                .HasKey(up => up.EmployeeId); // Ensure EmployeeId is the primary key for UserProfile

            builder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(u => u.UserProfile)  // One-to-one relationship with User
                .HasForeignKey<UserProfile>(up => up.EmployeeId)  // Foreign key on UserId in UserProfile
                .HasPrincipalKey<User>(u => u.EmployeeId);  // Use EmployeeId as the key in the relationship

            // Ensure that EmployeeId in User is not auto-generated (by default EF Core does not auto-generate string keys)
            builder.Entity<User>()
                .Property(u => u.EmployeeId)
                .IsRequired();  // Ensure EmployeeId is required and not null
        }
    }
}
