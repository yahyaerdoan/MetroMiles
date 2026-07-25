using Core.CrossCuttingConcernLayer.ExceptionHandlings.Types.Businesses;
using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Brands.Rules;

public class BrandBusinessRules : BaseBusinessRules
{
    private readonly IBrandRepository _brandRepository;

    public BrandBusinessRules(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task BrandNameCannotBeDuplicatedWhenInserted(string name)
    {
        var result = await _brandRepository.GetAsync(predicate: b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (result != null)
        {
            throw new BusinessException(BrandMessages.BrandNameExists);
        }
    }

    public static Brand BrandShouldExistWhenSelected(Brand? brand)
    {
        if (brand == null)
        {
            throw new BusinessException(BrandMessages.BrandNotExists);
        }

        return brand;
    }
}
