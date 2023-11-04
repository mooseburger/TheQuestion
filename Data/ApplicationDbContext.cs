using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TheQuestion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedUsers(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            var user = new IdentityUser()
            {
                Id = "b74ddd14-6340-4840-95c2-db12554843e5",
                UserName = "Admin",
                Email = "admin@example.com",
                LockoutEnabled = false,
                PhoneNumber = "1234567890"
            };

            user.NormalizedUserName = "admin";
            user.NormalizedEmail = "admin@example.com";
            user.PasswordHash = "AQAAAAEAACcQAAAAEMuTng1EgJaSNjVSRClL6Rqpo9wOnkSmFtCHQitPIEgHcKKkqA6zxLuS1p3C1529dg=="; // admin123
            user.ConcurrencyStamp = "8cf618d1-9ae6-475e-8791-8badf12f99d4";
            user.SecurityStamp = "6d5e79c5-0bcd-4711-ad9b-91bc19ca4cf4";
            user.EmailConfirmed = true;

            builder.Entity<IdentityUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "fab4fac1-c546-41de-aebc-a14da6895711", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Id = "c7b013f0-5201-4317-abd8-c211f91b7330", Name = "Reviewer", ConcurrencyStamp = "2", NormalizedName = "Reviewer" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "fab4fac1-c546-41de-aebc-a14da6895711", UserId = "b74ddd14-6340-4840-95c2-db12554843e5" }
            );
        }
    }
}