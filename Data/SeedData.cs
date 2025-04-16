using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CompanyPhonebook.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger<SeedData> logger)
        {
            // Get required services
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Define roles to create
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;



            // Create roles if they don't exist
            foreach (var roleName in roleNames)
            {
                try
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (roleExist)
                    {
                        logger.LogInformation($"Role '{roleName}' already exists.");
                    }
                    else
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                        if (roleResult.Succeeded)
                        {
                            logger.LogInformation($"Role '{roleName}' created successfully.");
                        }
                        else
                        {
                            logger.LogError($"Error creating role '{roleName}': {string.Join(", ", roleResult.Errors)}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating role: {RoleName}", roleName);
                }
            }

            // Optional: Seed an admin user
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

            try
            {
                if (adminUser == null)
                {
                    // Create an admin user if it doesn't exist
                    var user = new IdentityUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        EmailConfirmed = true, // Email is confirmed by default
                        LockoutEnabled = false  // Disable lockout for the admin user during seed
                    };



                    // Create the user with a hashed password
                    var result = await userManager.CreateAsync(user, "AdminPassword123!");

                    if (result.Succeeded)
                    {
                        // Add user to "Admin" role
                        await userManager.AddToRoleAsync(user, "Admin");
                        logger.LogInformation("Admin user created and assigned to 'Admin' role.");
                    }
                    else
                    {
                        logger.LogError($"Error creating admin user: {string.Join(", ", result.Errors)}");
                    }
                }
                else
                {
                    logger.LogInformation("Admin user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error creating admin user: {ex.Message}");
            }


        }
    }
}
