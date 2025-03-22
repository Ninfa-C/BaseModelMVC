using System.Reflection.Emit;
using HotelManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagment.Data
{
    public class HotelManagmentDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public HotelManagmentDbContext(DbContextOptions<HotelManagmentDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Camera> Camera { get; set; }
        public DbSet<Prenotazione> Prenotazioni { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Prenotazione>()
                .HasOne(p => p.Cliente)
                .WithMany(p => p.Prenotazione)
                .HasForeignKey(p => p.ClienteId);

            builder.Entity<Prenotazione>()
               .HasOne(p => p.Camera)
               .WithMany(p => p.Prenotazione)
               .HasForeignKey(p => p.CameraId);

            builder.Entity<Prenotazione>()
               .HasOne(p => p.User)
               .WithMany(p => p.Prenotazione)
               .HasForeignKey(p => p.UserId);

            builder.Entity<Prenotazione>()
            .ToTable(tb => tb.HasCheckConstraint("CK_Prenotazione_Stato",
                "Stato IN ('Confermata', 'Check-in Effettuato', 'Check-out Effettuato', 'Annullata', 'No-Show')"));

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
               new ApplicationRole { Id = "2", Name = "Manager", NormalizedName = "MANAGER", ConcurrencyStamp = "2" },
               new ApplicationRole { Id = "3", Name = "Receptionist", NormalizedName = "RECEPTIONIST", ConcurrencyStamp = "3" }
            );
        }
    }
}
