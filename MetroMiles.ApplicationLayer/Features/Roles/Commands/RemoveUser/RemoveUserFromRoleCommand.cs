using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveUser;

public class RemoveUserFromRoleCommand : IRequest<OperationResult>, ISecureAddRequest
{
    public Guid? UserId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class RemoveUserFromRoleCommandHandler(UserManager<User> userManager) : IRequestHandler<RemoveUserFromRoleCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString()!);
            if (user is null)
            {
                return Result.NotFound("User not found.");
            }

            var removeResult = await userManager.RemoveFromRoleAsync(user, request.RoleName);
            if (!removeResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", removeResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
