using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Models.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Models.Rules;

public class ModelBusinessRules(
    IModelRepository modelRepository,
    IBrandRepository brandRepository,
    IFuelRepository fuelRepository,
    ITransmissionRepository transmissionRepository,
    ICarRepository carRepository) : BaseBusinessRules
{
    private readonly IModelRepository _modelRepository = modelRepository;
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IFuelRepository _fuelRepository = fuelRepository;
    private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
    private readonly ICarRepository _carRepository = carRepository;

    public static IOperationResult<Model> ModelShouldExistWhenSelected(Model? model)
        => model is null ? Result.NotFound<Model>(ModelMessages.ModelNotExists) : Result.Success(model);

    public async Task<IOperationResult> ModelNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _modelRepository.AnyAsync(predicate: m => m.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(ModelMessages.ModelNameExists) : Result.Success();
    }

    public async Task<IOperationResult> ModelNameCannotBeDuplicatedWhenUpdated(Guid id, string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _modelRepository.AnyAsync(predicate: m => m.Id != id && m.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(ModelMessages.ModelNameExists) : Result.Success();
    }

    public async Task<IOperationResult> BrandIdShouldExistWhenSelected(Guid brandId)
    {
        var doesExist = await _brandRepository.AnyAsync(predicate: b => b.Id == brandId);
        return doesExist ? Result.Success() : Result.NotFound(ModelMessages.BrandNotExists);
    }

    public async Task<IOperationResult> FuelIdShouldExistWhenSelected(Guid fuelId)
    {
        var doesExist = await _fuelRepository.AnyAsync(predicate: f => f.Id == fuelId);
        return doesExist ? Result.Success() : Result.NotFound(ModelMessages.FuelNotExists);
    }

    public async Task<IOperationResult> TransmissionIdShouldExistWhenSelected(Guid transmissionId)
    {
        var doesExist = await _transmissionRepository.AnyAsync(predicate: t => t.Id == transmissionId);
        return doesExist ? Result.Success() : Result.NotFound(ModelMessages.TransmissionNotExists);
    }

    public async Task<IOperationResult> ModelShouldNotBeInUseWhenDeleted(Guid id)
    {
        var isInUse = await _carRepository.AnyAsync(predicate: c => c.ModelId == id);
        return isInUse ? Result.BadRequest(ModelMessages.ModelInUse) : Result.Success();
    }

    public static IOperationResult<Model> ModelShouldBeDeletedWhenRestored(Model? model)
    {
        if (model is null)
        {
            return Result.NotFound<Model>(ModelMessages.ModelNotExists);
        }
        return model.DeletedDate.HasValue ? Result.Success(model) : Result.BadRequest<Model>(ModelMessages.ModelNotDeleted);
    }

    public static IOperationResult ModelRowVersionShouldMatchWhenUpdated(Model existingModel, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingModel.RowVersion, clientRowVersion, ModelMessages.ModelModifiedByAnotherUser);
}
