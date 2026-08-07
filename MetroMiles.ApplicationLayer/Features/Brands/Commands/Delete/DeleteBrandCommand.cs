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

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Delete;

public class DeleteBrandCommand : IRequest<OperationDataResult<DeletedBrandResponse>>, ICacheRemoveRequest, ISecureAddRequest
{
    // DeleteBrandCommand & ICacheRemoveRequest Properties
    public Guid Id { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    public string[] Roles => [BrandsOperationClaims.Admin, BrandsOperationClaims.Write, BrandsOperationClaims.Delete];

    public class DeleteBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules) : IRequestHandler<DeleteBrandCommand, OperationDataResult<DeletedBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IMapper _mapper = mapper;
        private readonly BrandBusinessRules _brandBusinessRules = brandBusinessRules;

        public async Task<OperationDataResult<DeletedBrandResponse>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldExistWhenSelected(existingBrand);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedBrandResponse>();
            }

            var inUseCheck = await _brandBusinessRules.BrandShouldNotBeInUseWhenDeleted(request.Id);
            if (!inUseCheck.IsSuccessful)
            {
                return inUseCheck.ToErrorDataResult<DeletedBrandResponse>();
            }

            await _brandRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedBrandResponse>(existenceCheck.Data));
        }
    }
}
