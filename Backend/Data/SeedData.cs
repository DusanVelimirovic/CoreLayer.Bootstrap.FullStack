using Backend.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CoreLayerDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await context.Database.MigrateAsync(); // Ensure DB is up-to-date

            // Seed Module
            if (!context.Modules.Any())
            {
                context.Modules.Add(new ModuleI { ModuleName = "Test Module" });
                await context.SaveChangesAsync();
            }

            // Seed Role
            var roleName = "Admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }

            // Seed User
            var testEmail = "admin@test.com";
            var user = await userManager.FindByEmailAsync(testEmail);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@test.com",
                    Email = testEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "P@ssword1");
                await userManager.AddToRoleAsync(user, roleName);
            }

            // Assign ModuleAccessControl
            var module = await context.Modules.FirstAsync();
            var role = await roleManager.FindByNameAsync(roleName);

            if (!context.ModuleAccessControls.Any(mac => mac.ModuleId == module.ModuleId && mac.RoleId == role.Id))
            {
                context.ModuleAccessControls.Add(new ModuleAccessControl
                {
                    ModuleId = module.ModuleId,
                    RoleId = role.Id
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
