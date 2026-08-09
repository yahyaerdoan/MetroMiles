using System.Security.Claims;
using Core.SecurityLayer.Extensions;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace MetroMiles.ApplicationLayer.Features.Auths.Services;

// Single place that turns a User into the claim set the JWT (and, transitively,
// AuthorizationAddingBehavior's ISecureAddRequest.Roles check) relies on. Both LoginCommand and
// RefreshTokenCommand need identical claims, so this exists to keep them from drifting apart.
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

        // Fine-grained permission claims (e.g. "brands.delete") are stored as RoleClaims with
        // ClaimType == ClaimTypes.Role, so they land in the token as additional role claims —
        // ISecureAddRequest.Roles checks never need to know the difference between an actual
        // Identity role name and one of these permission strings.
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
