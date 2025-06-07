using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pab.Domain.Identity;
using System;
using System.Threading.Tasks;

namespace Pab.API.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();

            // Tworzenie ról
            string[] roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }

            // Tworzenie konta Admin, jeśli nie istnieje
            var adminEmail = "admin@pab.local";
            if (await userMgr.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userMgr.CreateAsync(admin, "Admin!123");
                if (result.Succeeded)
                {
                    await userMgr.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
