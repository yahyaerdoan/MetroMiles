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

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdateBrandCommand : IRequest<OperationDataResult<UpdatedBrandResponse>>, ICacheRemoveRequest, ISecureAddRequest
{
    #region UpdateBrandCommand & ICacheRemoveRequest Properties
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public byte[]? RowVersion { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    public string[] Roles => [BrandsOperationClaims.Admin, BrandsOperationClaims.Write, BrandsOperationClaims.Update];
    #endregion

    public class UpdateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules) : IRequestHandler<UpdateBrandCommand, OperationDataResult<UpdatedBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IMapper _mapper = mapper;
        private readonly BrandBusinessRules _brandBusinessRules = brandBusinessRules;

        public async Task<OperationDataResult<UpdatedBrandResponse>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldExistWhenSelected(existingBrand);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedBrandResponse>();
            }

            var versionCheck = BrandBusinessRules.BrandRowVersionShouldMatchWhenUpdated(existenceCheck.Data, request.RowVersion);
            if (!versionCheck.IsSuccessful)
            {
                return versionCheck.ToErrorDataResult<UpdatedBrandResponse>();
            }

            var duplicateCheck = await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenUpdated(request.Id, request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<UpdatedBrandResponse>();
            }

            var brand = _mapper.Map(request, existenceCheck.Data);
            await _brandRepository.UpdateAsync(brand);
            return Result.Success(_mapper.Map<UpdatedBrandResponse>(brand));
        }
    }
}
