using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.Delete;

public class DeleteRoleCommand : IRequest<OperationResult>, ISecureAddRequest
{
    public Guid? Id { get; set; }

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class DeleteRoleCommandHandler(RoleManager<Role> roleManager) : IRequestHandler<DeleteRoleCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.Id.ToString()!);
            if (role is null)
            {
                return Result.NotFound("Role not found.");
            }

            var deleteResult = await roleManager.DeleteAsync(role);
            if (!deleteResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", deleteResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
