using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Rules;

public class TransmissionBusinessRules(ITransmissionRepository transmissionRepository, IModelRepository modelRepository) : BaseBusinessRules
{
    private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
    private readonly IModelRepository _modelRepository = modelRepository;

    public static IOperationResult<Transmission> TransmissionShouldExistWhenSelected(Transmission? transmission)
        => transmission is null ? Result.NotFound<Transmission>(TransmissionMessages.TransmissionNotExists) : Result.Success(transmission);

    public async Task<IOperationResult> TransmissionNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _transmissionRepository.AnyAsync(predicate: t => t.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(TransmissionMessages.TransmissionNameExists) : Result.Success();
    }

    public async Task<IOperationResult> TransmissionNameCannotBeDuplicatedWhenUpdated(Guid id, string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _transmissionRepository.AnyAsync(predicate: t => t.Id != id && t.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(TransmissionMessages.TransmissionNameExists) : Result.Success();
    }

    public async Task<IOperationResult> TransmissionShouldNotBeInUseWhenDeleted(Guid id)
    {
        var isInUse = await _modelRepository.AnyAsync(predicate: m => m.TransmissionId == id);
        return isInUse ? Result.BadRequest(TransmissionMessages.TransmissionInUse) : Result.Success();
    }

    public static IOperationResult<Transmission> TransmissionShouldBeDeletedWhenRestored(Transmission? transmission)
    {
        if (transmission is null)
        {
            return Result.NotFound<Transmission>(TransmissionMessages.TransmissionNotExists);
        }
        return transmission.DeletedDate.HasValue ? Result.Success(transmission) : Result.BadRequest<Transmission>(TransmissionMessages.TransmissionNotDeleted);
    }

    public static IOperationResult TransmissionRowVersionShouldMatchWhenUpdated(Transmission existingTransmission, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingTransmission.RowVersion, clientRowVersion, TransmissionMessages.TransmissionModifiedByAnotherUser);
}
