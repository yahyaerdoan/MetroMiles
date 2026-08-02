using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;
using static MetroMiles.ApplicationLayer.Features.Users.Constants.UsersOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Create;

public class CreateUserCommand : IRequest<OperationDataResult<CreatedUserResponse>>, ISecureAddRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
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
    public string[] Roles => new[] { Admin, Write, Add };

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationDataResult<CreatedUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<OperationDataResult<CreatedUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailCheck = await _userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email);
            if (!emailCheck.IsSuccessful)
                return emailCheck.ToErrorDataResult<CreatedUserResponse>();

            User user = _mapper.Map<User>(request);
            user.Status = true;

            HashingHelper.CreatePasswordHash(
                request.Password,
                passwordHash: out byte[] passwordHash,
                passwordSalt: out byte[] passwordSalt
            );
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            User createdUser = await _userRepository.AddAsync(user);

            return Result.Success(_mapper.Map<CreatedUserResponse>(createdUser));
        }
    }
}
