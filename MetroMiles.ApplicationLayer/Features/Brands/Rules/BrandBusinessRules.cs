using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Brands.Rules;

public class BrandBusinessRules(IBrandRepository brandRepository, IModelRepository modelRepository) : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository = brandRepository;
    private readonly IModelRepository _modelRepository = modelRepository;

    public async Task<IOperationResult> BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var result = await _brandRepository.GetAsync(predicate: b => b.NormalizedName == normalizedName);
        return result != null ? Result.BadRequest(BrandMessages.BrandNameExists) : Result.Success();
    }

    public async Task<IOperationResult> BrandNameCannotBeDuplicatedWhenUpdated(Guid id, string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var doesExist = await _brandRepository.AnyAsync(predicate: b => b.Id != id && b.NormalizedName == normalizedName);
        return doesExist ? Result.BadRequest(BrandMessages.BrandNameExists) : Result.Success();
    }

    public static IOperationResult<Brand> BrandShouldExistWhenSelected(Brand? brand)
        => brand is null ? Result.NotFound<Brand>(BrandMessages.BrandNotExists) : Result.Success(brand);

    public async Task<IOperationResult> BrandShouldNotBeInUseWhenDeleted(Guid id)
    {
        var isInUse = await _modelRepository.AnyAsync(predicate: m => m.BrandId == id);
        return isInUse ? Result.BadRequest(BrandMessages.BrandInUse) : Result.Success();
    }

    public static IOperationResult<Brand> BrandShouldBeDeletedWhenRestored(Brand? brand)
    {
        if (brand is null)
        {
            return Result.NotFound<Brand>(BrandMessages.BrandNotExists);
        }
        return brand.DeletedDate.HasValue ? Result.Success(brand) : Result.BadRequest<Brand>(BrandMessages.BrandNotDeleted);
    }

    public static IOperationResult BrandRowVersionShouldMatchWhenUpdated(Brand existingBrand, byte[]? clientRowVersion)
        => RowVersionShouldMatchWhenUpdated(existingBrand.RowVersion, clientRowVersion, BrandMessages.BrandModifiedByAnotherUser);
}
