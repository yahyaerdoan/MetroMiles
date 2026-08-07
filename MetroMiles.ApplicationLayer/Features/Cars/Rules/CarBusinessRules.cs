using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.DomainLayer.Entities.Enums;
using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Cars.Rules;

public class CarBusinessRules(IModelRepository modelRepository, ICarRepository carRepository) : BaseBusinessRules
{
    private readonly IModelRepository _modelRepository = modelRepository;
    private readonly ICarRepository _carRepository = carRepository;

    public static IOperationResult<Car> CarShouldExistWhenSelected(Car? car)
        => car is null ? Result.NotFound<Car>(CarMessages.CarNotExists) : Result.Success(car);

    public async Task<IOperationResult> ModelIdShouldExistWhenSelected(Guid modelId)
    {
        var doesExist = await _modelRepository.AnyAsync(predicate: m => m.Id == modelId);
        return doesExist ? Result.Success() : Result.NotFound(CarMessages.ModelNotExists);
    }

    public async Task<IOperationResult> PlateCannotBeDuplicatedWhenInserted(string plate)
    {
        var normalizedPlate = plate.ToUpperInvariant();
        var doesExist = await _carRepository.AnyAsync(predicate: c => c.NormalizedPlate == normalizedPlate);
        return doesExist ? Result.BadRequest(CarMessages.PlateExists) : Result.Success();
    }

    public async Task<IOperationResult> PlateCannotBeDuplicatedWhenUpdated(Guid id, string plate)
    {
        var normalizedPlate = plate.ToUpperInvariant();
        var doesExist = await _carRepository.AnyAsync(predicate: c => c.Id != id && c.NormalizedPlate == normalizedPlate);
        return doesExist ? Result.BadRequest(CarMessages.PlateExists) : Result.Success();
    }

    public static IOperationResult<Car> CarShouldBeDeletedWhenRestored(Car? car)
    {
        if (car is null)
        {
            return Result.NotFound<Car>(CarMessages.CarNotExists);
        }
        return car.DeletedDate.HasValue ? Result.Success(car) : Result.BadRequest<Car>(CarMessages.CarNotDeleted);
    }

    public static IOperationResult PlateAndModelCannotChangeWhileRented(Car existingCar, string newPlate, Guid newModelId)
    {
        var isChangingIdentity = !string.Equals(existingCar.Plate, newPlate, StringComparison.OrdinalIgnoreCase) || existingCar.ModelId != newModelId;
        var isLocked = existingCar.Status == CarStatus.Rented && isChangingIdentity;
        return isLocked ? Result.BadRequest(CarMessages.PlateAndModelCannotChangeWhileRented) : Result.Success();
    }

    public static IOperationResult CarRowVersionShouldMatchWhenUpdated(Car existingCar, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingCar.RowVersion, clientRowVersion, CarMessages.CarModifiedByAnotherUser);
}
