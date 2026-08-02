using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;

using MediatR;

using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<OperationResult>
{
    // Set by the controller from the caller's own JWT claims — never trust a client-supplied user id
    // here, or one authenticated user could change another user's password.
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

    public class ChangePasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ChangePasswordCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            User? user = await userRepository.GetAsync(predicate: u => u.Id == request.UserId, cancellationToken: cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(user);
            if (!existenceCheck.IsSuccessful)
                return (OperationResult)existenceCheck;

            var passwordCheck = UserBusinessRules.UserPasswordShouldBeMatched(existenceCheck.Data, request.CurrentPassword);
            if (!passwordCheck.IsSuccessful)
                return (OperationResult)passwordCheck;

            HashingHelper.CreatePasswordHash(request.NewPassword, out byte[] newHash, out byte[] newSalt);
            existenceCheck.Data.PasswordHash = newHash;
            existenceCheck.Data.PasswordSalt = newSalt;
            await userRepository.UpdateAsync(existenceCheck.Data);

            return Result.Success();
        }
    }
}
