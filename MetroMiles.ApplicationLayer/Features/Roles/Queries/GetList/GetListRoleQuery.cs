using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Constants;
using MediatR;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Roles.Queries.GetList;

public class GetListRoleQuery : IRequest<OperationDataResult<List<GetListRoleListItemDto>>>, ISecureAddRequest
{
    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class GetListRoleQueryHandler(RoleManager<Role> roleManager) : IRequestHandler<GetListRoleQuery, OperationDataResult<List<GetListRoleListItemDto>>>
    {
        public async Task<OperationDataResult<List<GetListRoleListItemDto>>> Handle(GetListRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await roleManager.Roles.ToListAsync(cancellationToken);

            List<GetListRoleListItemDto> response = [];
            foreach (var role in roles)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                response.Add(new GetListRoleListItemDto
                {
                    Id = role.Id,
                    Name = role.Name!,
                    Claims = [.. claims.Select(c => c.Value)],
                });
            }

            return Result.Success(response);
        }
    }
}
