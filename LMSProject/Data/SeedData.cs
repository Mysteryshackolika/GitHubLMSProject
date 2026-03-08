using Microsoft.AspNetCore.Identity;
using LMSProject.Models;

namespace LMSProject.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                // Rolları yarat
                string[] roles = { "Admin", "Teacher", "Student" };

                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Admin istifadəçisini yarat
                string adminEmail = "admin@lms.com";
                string adminPassword = "Admin@123";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FirstName = "Admin",
                        LastName = "User",
                        CreatedAt = DateTime.Now,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }
        }
    }
}