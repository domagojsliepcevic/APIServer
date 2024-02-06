using APIServer.Enums;
using APIServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIServer.Data
{
    public partial class DatabaseContext : IdentityUserContext<ApplicationUser>
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car>? Cars { get; set; }
        public virtual DbSet<Truck>? Trucks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call base method to configure Identity related entities

            

            // Configure ApplicationUser entity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                // Define primary key
                entity.HasKey(e => e.Id);

                // Configure other properties
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                entity.Property(e => e.PasswordHash).HasMaxLength(256);
                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(256);
                entity.Property(e => e.PhoneNumber).HasMaxLength(30);

                // Additional configurations
                // Add more configurations as needed for your application
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Cars");
                entity.Property(e => e.Id).HasColumnName("CarID");
                entity.Property(e => e.Make).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Model).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Year).HasMaxLength(4).IsUnicode(false);
                entity.Property(e => e.Color).HasMaxLength(25).IsUnicode(false);
                entity.Property(e => e.XmlData).HasMaxLength(255).IsUnicode(false); // Add XmlData property

            });

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.ToTable("Trucks");
                entity.Property(e => e.Id).HasColumnName("TruckID");
                entity.Property(e => e.Make).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Model).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.Year).HasMaxLength(4).IsUnicode(false);
                entity.Property(e => e.Color).HasMaxLength(25).IsUnicode(false);
                

            });




            // Seed AspNetUsers table with default admin user
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminEmail = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")["AdminEmail"];
            var adminPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteSettings")["AdminPassword"];

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "80c8b6b1-e2b6-45e8-b044-8f2178a90111", // primary key
                    UserName = "admin",
                    NormalizedUserName = adminEmail.ToUpper(),
                    PasswordHash = hasher.HashPassword(null, adminPassword),
                    Email = adminEmail,
                    NormalizedEmail = adminEmail.ToUpper(),
                    Role = Role.Admin
                }
            );

            OnModelCreatingPartial(modelBuilder);

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
