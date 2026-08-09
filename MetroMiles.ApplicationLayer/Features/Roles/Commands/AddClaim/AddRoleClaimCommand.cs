using System.Security.Claims;
using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.AddClaim;

// Adds a permission-style claim (e.g. "brands.delete") to a role. Stored with ClaimType ==
// ClaimTypes.Role so UserClaimsFactory surfaces it in the JWT the same way it surfaces the role's
// own name — ISecureAddRequest.Roles checks (AuthorizationAddingBehavior) never need to know the
// difference between an Identity role name and one of these permission strings.
public class AddRoleClaimCommand : IRequest<OperationResult>, ISecureAddRequest
{
    public Guid? RoleId { get; set; }

    public string ClaimValue { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class AddRoleClaimCommandHandler(RoleManager<Role> roleManager) : IRequestHandler<AddRoleClaimCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(AddRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.RoleId.ToString()!);
            if (role is null)
            {
                return Result.NotFound("Role not found.");
            }

            var existingClaims = await roleManager.GetClaimsAsync(role);
            if (existingClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == request.ClaimValue))
            {
                return Result.BadRequest("Role already has this claim.");
            }

            var addResult = await roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, request.ClaimValue));
            if (!addResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", addResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
