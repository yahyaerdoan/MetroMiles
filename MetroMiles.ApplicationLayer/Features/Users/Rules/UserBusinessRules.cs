using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;
using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Auths.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public static IOperationResult<User> UserShouldBeExistsWhenSelected(User? user)
        => user is null ? Result.NotFound<User>(AuthMessages.UserDontExists) : Result.Success(user);

    public async Task<IOperationResult> UserIdShouldBeExistsWhenSelected(int id)
    {
        bool doesExist = await _userRepository.AnyAsync(predicate: u => u.Id == id, enableTracking: false);
        return doesExist ? Result.Success() : Result.NotFound(AuthMessages.UserDontExists);
    }

    public static IOperationResult UserPasswordShouldBeMatched(User user, string password)
        => HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)
            ? Result.Success()
            : Result.BadRequest(AuthMessages.PasswordDontMatch);

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Email.ToLower() == email.ToLower(), enableTracking: false);
        return doesExists ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
    }

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenUpdate(int id, string email)
    {
        bool doesExists = await _userRepository.AnyAsync(predicate: u => u.Id != id && u.Email.ToLower() == email.ToLower(), enableTracking: false);
        return doesExists ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
    }
}
