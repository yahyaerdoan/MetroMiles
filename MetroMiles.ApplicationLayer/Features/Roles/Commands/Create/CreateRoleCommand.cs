using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.Create;

public class CreateRoleCommand : IRequest<OperationDataResult<CreatedRoleResponse>>, ISecureAddRequest
{
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class CreateRoleCommandHandler(RoleManager<Role> roleManager) : IRequestHandler<CreateRoleCommand, OperationDataResult<CreatedRoleResponse>>
    {
        public async Task<OperationDataResult<CreatedRoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (await roleManager.RoleExistsAsync(request.Name))
            {
                return Result.BadRequest<CreatedRoleResponse>("A role with this name already exists.");
            }

            Role role = new(request.Name);
            var createResult = await roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                return Result.BadRequest<CreatedRoleResponse>(string.Join(" ", createResult.Errors.Select(e => e.Description)));
            }

            return Result.Success(new CreatedRoleResponse(role.Id, role.Name!));
        }
    }
}
