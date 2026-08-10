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

public class GetListRoleQuery : IRequest<OperationDataResult<List<GetListRoleListItemResponse>>>, ISecureAddRequest
{
    [JsonIgnore]
    public string[] Roles => [GeneralOperationClaims.Admin];

    public class GetListRoleQueryHandler(RoleManager<Role> roleManager) : IRequestHandler<GetListRoleQuery, OperationDataResult<List<GetListRoleListItemResponse>>>
    {
        public async Task<OperationDataResult<List<GetListRoleListItemResponse>>> Handle(GetListRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await roleManager.Roles.ToListAsync(cancellationToken);

            List<GetListRoleListItemResponse> response = [];
            foreach (var role in roles)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                response.Add(new GetListRoleListItemResponse
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
