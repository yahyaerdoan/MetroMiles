using Core.CrossCuttingConcernLayer.ExceptionHandlings.Types;
using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.ApplicationLayer.Extensions.Rules;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Brand? result = await _brandRepository.GetAsync(predicate: b=> b.Name.ToLower() == name.ToLower());
        if (result != null) 
        {
            throw new BusinessException(BrandMessages.BrandNameExists);
        }
    }
}
