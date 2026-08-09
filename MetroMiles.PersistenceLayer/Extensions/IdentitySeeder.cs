using Core.SecurityLayer.Constants;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MetroMiles.PersistenceLayer.Extensions;

// Dev/first-run bootstrap — Identity's tables start empty, so nothing could log in without this.
public static class IdentitySeeder
{
    private const string AdminRoleName = "Admin";
    private const string AdminEmail = "admin@metromiles.com";

    // Never used past first run in a real environment — change it immediately after first login.
    private const string InitialAdminPassword = "Admin123!";

    public static async Task SeedIdentityDataAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        if (await roleManager.FindByNameAsync(AdminRoleName) is null)
        {
            // No RoleClaims needed — a role named "Admin" is already an all-access bypass.
            await roleManager.CreateAsync(new Role(GeneralOperationClaims.Admin));
        }

        if (await userManager.FindByEmailAsync(AdminEmail) is not null)
        {
            return;
        }

        User adminUser = new()
        {
            FirstName = "Admin",
            LastName = "MetroMiles",
            UserName = AdminEmail,
            Email = AdminEmail,
            EmailConfirmed = true,
        };

        var createResult = await userManager.CreateAsync(adminUser, InitialAdminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, AdminRoleName);
        }
    }
}
