using System.Security.Claims;
using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveClaim;

public class RemoveRoleClaimCommand : IRequest<OperationResult>, ISecureAddRequest
{
    public Guid? RoleId { get; set; }

    public string ClaimValue { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class RemoveRoleClaimCommandHandler(RoleManager<Role> roleManager) : IRequestHandler<RemoveRoleClaimCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(RemoveRoleClaimCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.RoleId.ToString()!);
            if (role is null)
            {
                return Result.NotFound("Role not found.");
            }

            var removeResult = await roleManager.RemoveClaimAsync(role, new Claim(ClaimTypes.Role, request.ClaimValue));
            if (!removeResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", removeResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
