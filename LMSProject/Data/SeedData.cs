using Microsoft.AspNetCore.Identity;
using LMSProject.Models;

namespace LMSProject.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Teacher", "Student" };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

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

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Proqramlaşdırma", Description = "Python, C#, Java və s.", Icon = "bi-code-slash", DisplayOrder = 1 },
                    new Category { Name = "Dizayn", Description = "UI/UX, Qrafik dizayn", Icon = "bi-palette", DisplayOrder = 2 },
                    new Category { Name = "Marketinq", Description = "Rəqəmsal marketinq, SEO", Icon = "bi-megaphone", DisplayOrder = 3 },
                    new Category { Name = "Biznes", Description = "Sahibkarlıq, menecment", Icon = "bi-briefcase", DisplayOrder = 4 }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Badges.Any())
            {
                var badges = new List<Badge>
                {
                    new Badge { Name = "İlk Kurs", Description = "İlk kursunuzu tamamladınız", Type = BadgeType.Course, Rarity = BadgeRarity.Common, PointsReward = 50, Criteria = "first_course", RequiredCount = 1 },
                    new Badge { Name = "Kurs Həvəskarı", Description = "5 kurs tamamladınız", Type = BadgeType.Course, Rarity = BadgeRarity.Rare, PointsReward = 100, Criteria = "five_courses", RequiredCount = 5 },
                    new Badge { Name = "Kurs Ustası", Description = "10 kurs tamamladınız", Type = BadgeType.Course, Rarity = BadgeRarity.Epic, PointsReward = 200, Criteria = "ten_courses", RequiredCount = 10 }
                };

                context.Badges.AddRange(badges);
                await context.SaveChangesAsync();
            }
        }
    }
}