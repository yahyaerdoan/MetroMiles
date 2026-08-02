using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Pipelines.Transactions.Abstractions;
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

using static MetroMiles.ApplicationLayer.Features.Brands.Constants.BrandsOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;

public class CreateBrandCommand : IRequest<OperationDataResult<CreatedBrandResponse>>, ITransactionAddRequest, ICacheRemoveRequest, ILogAddRequest, ISecureAddRequest
{
    #region CreateBrandCommand & ICacheRemoveRequest Properties
    public string Name { get; set; }
    public string Description { get; set; }
    public string CacheKey => "";
    public bool ByPassCache => false;
    public string? CacheGroupKey => "GetBrands";
    public string[] Roles => [Admin, Write, Add];
    #endregion

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, OperationDataResult<CreatedBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<OperationDataResult<CreatedBrandResponse>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var duplicateCheck = await _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            if (!duplicateCheck.IsSuccessful)
                return duplicateCheck.ToErrorDataResult<CreatedBrandResponse>();

            var brand = _mapper.Map<Brand>(request);
            await _brandRepository.AddAsync(brand);
            return Result.Success(_mapper.Map<CreatedBrandResponse>(brand));
        }
    }
}
