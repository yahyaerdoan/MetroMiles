using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Features.Brands.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;

public class RestoreBrandCommand : CacheRemovingSecuredCommand<Guid, RestoredBrandResponse>
{
    [JsonIgnore]
    public override string? CacheGroupKey => "GetBrands";
    [JsonIgnore]
    public override string[] Roles => BrandsOperationClaims.UpdateRoles;

    public class RestoreBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules) : IRequestHandler<RestoreBrandCommand, OperationDataResult<RestoredBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IMapper _mapper = mapper;
        private readonly BrandBusinessRules _brandBusinessRules = brandBusinessRules;

        public async Task<OperationDataResult<RestoredBrandResponse>> Handle(RestoreBrandCommand request, CancellationToken cancellationToken)
        {
            var existingBrand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldBeDeletedWhenRestored(existingBrand);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredBrandResponse>();
            }

            var duplicateCheck = await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, existenceCheck.Data.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<RestoredBrandResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _brandRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredBrandResponse>(existenceCheck.Data));
        }
    }
}
