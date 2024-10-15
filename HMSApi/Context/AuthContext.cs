using HMSApi.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HMSApi.Context
{
    public class AuthContext : IdentityDbContext<User>
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }


        public async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Doctor", "Nurse" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new User { UserName = "admin", Email = "admin@gmail.com", Role = "Admin" };

            var existingAdmin = await userManager.FindByEmailAsync(adminUser.Email);
            if (existingAdmin == null)
            {
                await userManager.CreateAsync(adminUser, "AdminPassword123!"); 
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}
