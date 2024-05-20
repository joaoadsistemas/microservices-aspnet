using GeekShopping.Authentication.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GeekShopping.Authentication.Context
{
    public class SystemDbContext : IdentityDbContext<User>
    {
        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Client", NormalizedName = "CLIENT" }
            );

            // Seed users and roles
            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Senha#123"),
                    SecurityStamp = string.Empty,
                    Name = "Admin"
                },
                new User
                {
                    Id = "2",
                    UserName = "client",
                    NormalizedUserName = "CLIENT",
                    Email = "client@gmail.com",
                    NormalizedEmail = "CLIENT@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Senha#123"),
                    SecurityStamp = string.Empty,
                    Name = "Client",
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" }, // Admin role
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" }  // Lawyer role
            );




        }
    }
}
