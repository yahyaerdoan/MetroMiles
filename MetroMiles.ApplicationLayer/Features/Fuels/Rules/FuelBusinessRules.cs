using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Rules;

public class FuelBusinessRules(IFuelRepository fuelRepository, IModelRepository modelRepository) : BaseBusinessRules
{
    private readonly IFuelRepository _fuelRepository = fuelRepository;
    private readonly IModelRepository _modelRepository = modelRepository;

    public static IOperationResult<Fuel> FuelShouldExistWhenSelected(Fuel? fuel)
        => fuel is null ? Result.NotFound<Fuel>(FuelMessages.FuelNotExists) : Result.Success(fuel);

    public async Task<IOperationResult> FuelNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _fuelRepository.AnyAsync(predicate: f => f.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(FuelMessages.FuelNameExists) : Result.Success();
    }

    public async Task<IOperationResult> FuelNameCannotBeDuplicatedWhenUpdated(Guid id, string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _fuelRepository.AnyAsync(predicate: f => f.Id != id && f.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(FuelMessages.FuelNameExists) : Result.Success();
    }

    public async Task<IOperationResult> FuelShouldNotBeInUseWhenDeleted(Guid id)
    {
        var isInUse = await _modelRepository.AnyAsync(predicate: m => m.FuelId == id);
        return isInUse ? Result.BadRequest(FuelMessages.FuelInUse) : Result.Success();
    }

    public static IOperationResult<Fuel> FuelShouldBeDeletedWhenRestored(Fuel? fuel)
    {
        if (fuel is null)
        {
            return Result.NotFound<Fuel>(FuelMessages.FuelNotExists);
        }
        return fuel.DeletedDate.HasValue ? Result.Success(fuel) : Result.BadRequest<Fuel>(FuelMessages.FuelNotDeleted);
    }

    public static IOperationResult FuelRowVersionShouldMatchWhenUpdated(Fuel existingFuel, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingFuel.RowVersion, clientRowVersion, FuelMessages.FuelModifiedByAnotherUser);
}
