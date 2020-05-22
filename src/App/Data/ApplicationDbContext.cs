using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            string[] roles = { "Admin" };

            var id = Guid.NewGuid().ToString();

            foreach (var role in roles)
            {
                modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
                {
                    Id = id,
                    Name = role,
                    NormalizedName = role
                });
            }

            string email = "admin@admin.com";

            var passwordHasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id= id,
                Email = email,
                NormalizedEmail = email,
                UserName = email,
                NormalizedUserName = email,
                PasswordHash = passwordHasher.HashPassword(null, "Teste@123"),
                EmailConfirmed = true,
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = id,
                UserId = id
            });
        }
    }
}
