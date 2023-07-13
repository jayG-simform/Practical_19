using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;

namespace Practical_19.Data
{
    public class AppDbContext : IdentityDbContext<AppAuthentiction>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Students> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "1",
                Name = "Admin"
            },
            new IdentityRole()
            {
                Id = "2",
                Name = "User"
            });
            builder.Entity<Users>().HasData(new Users()
            {
                Id = 1,
                UserName = "Admin",
                Email = "Admin@gmail.com",
                Address = "Ahemdabad, Gujrat",
                MobileNumber = "7383751559",
                Password = "Admin@123"
            });

            builder.Entity<IdentityUserRole<string>>().HasData(
                        new IdentityUserRole<string>
                        {
                            RoleId = "1",
                            UserId = "1"
                        }
                    );
        }

    }
}
