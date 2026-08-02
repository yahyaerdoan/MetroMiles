using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Rules;

public class FuelBusinessRules(IFuelRepository fuelRepository) : BaseBusinessRules
{
    private readonly IFuelRepository _fuelRepository = fuelRepository;

    public static IOperationResult<Fuel> FuelShouldExistWhenSelected(Fuel? fuel)
        => fuel is null ? Result.NotFound<Fuel>(FuelMessages.FuelNotExists) : Result.Success(fuel);

    public async Task<IOperationResult> FuelNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _fuelRepository.AnyAsync(predicate: f => f.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(FuelMessages.FuelNameExists) : Result.Success();
    }
}
