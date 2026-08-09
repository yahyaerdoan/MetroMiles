using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.AssignUser;

public class AssignUserToRoleCommand : IRequest<OperationResult>, ISecureAddRequest
{
    public Guid? UserId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class AssignUserToRoleCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager) : IRequestHandler<AssignUserToRoleCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString()!);
            if (user is null)
            {
                return Result.NotFound("User not found.");
            }

            if (!await roleManager.RoleExistsAsync(request.RoleName))
            {
                return Result.NotFound("Role not found.");
            }

            if (await userManager.IsInRoleAsync(user, request.RoleName))
            {
                return Result.BadRequest("User already has this role.");
            }

            var addResult = await userManager.AddToRoleAsync(user, request.RoleName);
            if (!addResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", addResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
