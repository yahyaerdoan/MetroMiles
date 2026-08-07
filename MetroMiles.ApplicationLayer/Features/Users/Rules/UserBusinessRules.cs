using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;
using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Auths.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Users.Rules;

public class UserBusinessRules(IUserRepository userRepository) : BaseBusinessRules
{
    private readonly IUserRepository _userRepository = userRepository;

    public static IOperationResult<User> UserShouldBeExistsWhenSelected(User? user)
        => user is null ? Result.NotFound<User>(AuthMessages.UserDontExists) : Result.Success(user);

    public async Task<IOperationResult> UserIdShouldBeExistsWhenSelected(int id)
    {
        var doesExist = await _userRepository.AnyAsync(predicate: u => u.Id == id, enableTracking: false);
        return doesExist ? Result.Success() : Result.NotFound(AuthMessages.UserDontExists);
    }

    public static IOperationResult UserPasswordShouldBeMatched(User user, string password)
        => HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)
            ? Result.Success()
            : Result.BadRequest(AuthMessages.PasswordDontMatch);

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenInsert(string email)
    {
        var normalizedEmail = email.ToUpperInvariant();
        var doesExists = await _userRepository.AnyAsync(predicate: u => u.NormalizedEmail == normalizedEmail, enableTracking: false);
        return doesExists ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
    }

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenUpdate(int id, string email)
    {
        var normalizedEmail = email.ToUpperInvariant();
        var doesExists = await _userRepository.AnyAsync(predicate: u => u.Id != id && u.NormalizedEmail == normalizedEmail, enableTracking: false);
        return doesExists ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
    }

    public static IOperationResult<User> UserShouldBeDeletedWhenRestored(User? user)
    {
        if (user is null)
        {
            return Result.NotFound<User>(AuthMessages.UserDontExists);
        }
        return user.DeletedDate.HasValue ? Result.Success(user) : Result.BadRequest<User>(AuthMessages.UserNotDeleted);
    }

    public static IOperationResult UserRowVersionShouldMatchWhenUpdated(User existingUser, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingUser.RowVersion, clientRowVersion, AuthMessages.UserModifiedByAnotherUser);
}
