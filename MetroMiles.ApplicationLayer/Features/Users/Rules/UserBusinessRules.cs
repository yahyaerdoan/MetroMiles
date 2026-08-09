using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Auths.Constants;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Users.Rules;

public class UserBusinessRules(UserManager<User> userManager) : BaseBusinessRules
{
    private readonly UserManager<User> _userManager = userManager;

    public static IOperationResult<User> UserShouldBeExistsWhenSelected(User? user)
        => user is null ? Result.NotFound<User>(AuthMessages.UserDontExists) : Result.Success(user);

    public async Task<IOperationResult> UserIdShouldBeExistsWhenSelected(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return user is not null ? Result.Success() : Result.NotFound(AuthMessages.UserDontExists);
    }

    public async Task<IOperationResult> UserPasswordShouldBeMatched(User user, string password)
        => await _userManager.CheckPasswordAsync(user, password)
            ? Result.Success()
            : Result.BadRequest(AuthMessages.PasswordDontMatch);

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenInsert(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        return existingUser is not null ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
    }

    public async Task<IOperationResult> UserEmailShouldNotExistsWhenUpdate(Guid id, string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        return existingUser is not null && existingUser.Id != id ? Result.BadRequest(AuthMessages.UserMailAlreadyExists) : Result.Success();
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
