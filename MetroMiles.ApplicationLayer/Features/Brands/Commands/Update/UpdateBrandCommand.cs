using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Brands.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

using MetroMiles.ApplicationLayer.Features.Brands.Constants;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdateBrandCommand : IRequest<OperationDataResult<UpdatedBrandResponse>>, ICacheRemoveRequest, ISecureAddRequest
{
    #region UpdateBrandCommand & ICacheRemoveRequest Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    public string[] Roles => [BrandsOperationClaims.Admin, BrandsOperationClaims.Write, BrandsOperationClaims.Update];
    #endregion

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, OperationDataResult<UpdatedBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<OperationDataResult<UpdatedBrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            Brand? existingBrand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldExistWhenSelected(existingBrand);
            if (!existenceCheck.IsSuccessful)
                return existenceCheck.ToErrorDataResult<UpdatedBrandResponse>();

            Brand brand = _mapper.Map(request, existenceCheck.Data);
            await _brandRepository.UpdateAsync(brand);
            return Result.Success(_mapper.Map<UpdatedBrandResponse>(brand));
        }
    }
}
