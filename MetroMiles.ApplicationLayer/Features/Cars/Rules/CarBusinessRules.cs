using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Cars.Rules;

public class CarBusinessRules(IModelRepository modelRepository) : BaseBusinessRules
{
    private readonly IModelRepository _modelRepository = modelRepository;

    public static IOperationResult<Car> CarShouldExistWhenSelected(Car? car)
        => car is null ? Result.NotFound<Car>(CarMessages.CarNotExists) : Result.Success(car);

    public async Task<IOperationResult> ModelIdShouldExistWhenSelected(Guid modelId)
    {
        var doesExist = await _modelRepository.AnyAsync(predicate: m => m.Id == modelId);
        return doesExist ? Result.Success() : Result.NotFound(CarMessages.ModelNotExists);
    }
}
