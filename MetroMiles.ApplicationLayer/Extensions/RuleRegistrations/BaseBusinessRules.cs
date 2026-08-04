using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;

public class BaseBusinessRules
{
    // A null/empty client-submitted version means the caller isn't opting into concurrency
    // checking, so it's allowed through unconditionally — keeps the check backward-compatible
    // for any caller that doesn't (yet) round-trip RowVersion.
    protected static IOperationResult RowVersionShouldMatchWhenUpdated(byte[]? currentVersion, byte[]? clientVersion, string mismatchMessage)
    {
        if (clientVersion is null || clientVersion.Length == 0)
        {
            return Result.Success();
        }
        return currentVersion is not null && currentVersion.AsSpan().SequenceEqual(clientVersion)
            ? Result.Success()
            : Result.Conflict(mismatchMessage);
    }
}
