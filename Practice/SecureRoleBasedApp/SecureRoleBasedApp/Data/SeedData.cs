using Microsoft.AspNetCore.Identity;

namespace SecureRoleBasedApp.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            var admin =
                await userManager.FindByNameAsync("admin");

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin"
                };

                await userManager.CreateAsync(
                    admin,
                    "Admin@123");

                await userManager.AddToRoleAsync(
                    admin,
                    "Admin");
            }

            var user =
                await userManager.FindByNameAsync("user1");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "user1"
                };

                await userManager.CreateAsync(
                    user,
                    "User@123");

                await userManager.AddToRoleAsync(
                    user,
                    "User");
            }
        }
    }
}