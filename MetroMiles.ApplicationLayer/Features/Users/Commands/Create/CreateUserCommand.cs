using System.Text.Json.Serialization;
using AutoMapper;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Pipelines.Transactions.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;
using static MetroMiles.ApplicationLayer.Features.Users.Constants.UsersOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Create;

public class CreateUserCommand : SecuredRequest<CreatedUserResponse>, ITransactionAddRequest, ILogAddRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    [SensitiveData]
    public string Password { get; set; }

    public CreateUserCommand()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
    }

    public CreateUserCommand(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    [JsonIgnore]
    public override string[] Roles => [Admin, Write, Add];

    public class CreateUserCommandHandler(UserManager<User> userManager, IMapper mapper, UserBusinessRules userBusinessRules) : IRequestHandler<CreateUserCommand, OperationDataResult<CreatedUserResponse>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly UserBusinessRules _userBusinessRules = userBusinessRules;

        public async Task<OperationDataResult<CreatedUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailCheck = await _userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email);
            if (!emailCheck.IsSuccessful)
            {
                return emailCheck.ToErrorDataResult<CreatedUserResponse>();
            }

            User user = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                Email = request.Email,
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Result.BadRequest<CreatedUserResponse>(string.Join(" ", createResult.Errors.Select(e => e.Description)));
            }

            return Result.Success(_mapper.Map<CreatedUserResponse>(user));
        }
    }
}
