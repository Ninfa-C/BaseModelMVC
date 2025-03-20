using BaseModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseModel.Data
{
    public class BaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUserRole>()
                .Property(p => p.Date)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired(true);

            builder.Entity<ApplicationUserRole>()
                .HasOne(a => a.Role)
                .WithMany(u => u.ApplicationUserRole)
                .HasForeignKey(a => a.RoleId);
            builder.Entity<ApplicationUserRole>()
                .HasOne(a => a.User)
                .WithMany(u => u.ApplicationUserRole)
                .HasForeignKey(a => a.UserId);
            builder.Entity<ApplicationRole>().HasData(
               new ApplicationRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1" },
               new ApplicationRole { Id = "2", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "2" },
               new ApplicationRole { Id = "3", Name = "Moderator", NormalizedName = "MODERATOR", ConcurrencyStamp = "3" }
            );
        }
    }
}
