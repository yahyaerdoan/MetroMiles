using System.Text.Json.Serialization;
using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;

public class GetByIdUserQuery : IRequest<OperationDataResult<GetByIdUserResponse>>, ISecureAddRequest
{
    public Guid? Id { get; set; }

    [JsonIgnore]
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Read];

    public class GetByIdUserQueryHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<GetByIdUserQuery, OperationDataResult<GetByIdUserResponse>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdUserResponse>> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString()!);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(user);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdUserResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdUserResponse>(existenceCheck.Data));
        }
    }
}
