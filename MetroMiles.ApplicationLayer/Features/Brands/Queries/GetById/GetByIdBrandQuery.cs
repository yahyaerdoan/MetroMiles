using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Brands.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;

public class GetByIdBrandQuery : IRequest<OperationDataResult<GetByIdBrandResponse>>
{
    public Guid Id { get; set; }

    public class GetByIdBrandQueryHandler : IRequestHandler<GetByIdBrandQuery, OperationDataResult<GetByIdBrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetByIdBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<OperationDataResult<GetByIdBrandResponse>> Handle(GetByIdBrandQuery request, CancellationToken cancellationToken)
        {
            Brand? brand = await _brandRepository.GetAsync(predicate: b => b.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = BrandBusinessRules.BrandShouldExistWhenSelected(brand);
            if (!existenceCheck.IsSuccessful)
                return existenceCheck.ToErrorDataResult<GetByIdBrandResponse>();

            return Result.Success(_mapper.Map<GetByIdBrandResponse>(existenceCheck.Data));
        }
    }
}
