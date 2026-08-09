using System.Security.Claims;
using Core.SecurityLayer.Extensions;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace MetroMiles.ApplicationLayer.Features.Auths.Services;

// Shared by LoginCommand and RefreshTokenCommand so their JWT claims never drift apart.
public class UserClaimsFactory(UserManager<User> userManager, RoleManager<Role> roleManager)
{
    public async Task<List<Claim>> CreateClaimsAsync(User user)
    {
        List<Claim> claims = [];
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email!);
        claims.AddName($"{user.FirstName} {user.LastName}");

        var roleNames = await userManager.GetRolesAsync(user);
        claims.AddRoles([.. roleNames]);

        // Permission claims (e.g. "brands.delete") are RoleClaims of type ClaimTypes.Role.
        foreach (var roleName in roleNames)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                continue;
            }

            var roleClaims = await roleManager.GetClaimsAsync(role);
            claims.AddRange(roleClaims.Where(c => c.Type == ClaimTypes.Role));
        }

        return claims;
    }
}
