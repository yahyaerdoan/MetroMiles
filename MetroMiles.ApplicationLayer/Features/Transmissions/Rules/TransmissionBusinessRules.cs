using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Rules;

public class TransmissionBusinessRules(ITransmissionRepository transmissionRepository) : BaseBusinessRules
{
    private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;

    public static IOperationResult<Transmission> TransmissionShouldExistWhenSelected(Transmission? transmission)
        => transmission is null ? Result.NotFound<Transmission>(TransmissionMessages.TransmissionNotExists) : Result.Success(transmission);

    public async Task<IOperationResult> TransmissionNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _transmissionRepository.AnyAsync(predicate: t => t.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(TransmissionMessages.TransmissionNameExists) : Result.Success();
    }
}
