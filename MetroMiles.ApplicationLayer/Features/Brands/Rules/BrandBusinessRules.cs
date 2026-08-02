using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Brands.Rules;

public class BrandBusinessRules(IBrandRepository brandRepository) : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository = brandRepository;

    public async Task<IOperationResult> BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        var normalizedName = name.ToUpperInvariant();
        var result = await _brandRepository.GetAsync(predicate: b => b.NormalizedName == normalizedName);
        return result != null ? Result.BadRequest(BrandMessages.BrandNameExists) : Result.Success();
    }

    public static IOperationResult<Brand> BrandShouldExistWhenSelected(Brand? brand)
        => brand is null ? Result.NotFound<Brand>(BrandMessages.BrandNotExists) : Result.Success(brand);
}
