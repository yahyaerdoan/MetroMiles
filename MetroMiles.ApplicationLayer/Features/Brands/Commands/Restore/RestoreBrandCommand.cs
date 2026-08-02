using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Features.Brands.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;

public class RestoreBrandCommand : IRequest<OperationDataResult<RestoredBrandResponse>>, ICacheRemoveRequest, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    public string[] Roles => [BrandsOperationClaims.Admin, BrandsOperationClaims.Write, BrandsOperationClaims.Update];

    public class RestoreBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper) : IRequestHandler<RestoreBrandCommand, OperationDataResult<RestoredBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<RestoredBrandResponse>> Handle(RestoreBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldBeDeletedWhenRestored(existingBrand);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredBrandResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _brandRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredBrandResponse>(existenceCheck.Data));
        }
    }
}
