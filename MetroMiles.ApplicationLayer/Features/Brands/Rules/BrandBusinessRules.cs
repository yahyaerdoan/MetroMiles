using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Brands.Rules;

public class BrandBusinessRules : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository;

    public BrandBusinessRules(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IOperationResult> BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        // ToLower() (no CultureInfo/StringComparison overload) is the one case-folding form EF Core
        // translates to SQL (LOWER(...)) — it works regardless of the column's collation, unlike
        // EF.Functions.Like/Contains/== which only end up case-insensitive if the collation is.
        var result = await _brandRepository.GetAsync(predicate: b => b.Name.ToLower() == name.ToLower());
        return result != null ? Result.BadRequest(BrandMessages.BrandNameExists) : Result.Success();
    }

    public static IOperationResult<Brand> BrandShouldExistWhenSelected(Brand? brand)
        => brand is null ? Result.NotFound<Brand>(BrandMessages.BrandNotExists) : Result.Success(brand);
}
