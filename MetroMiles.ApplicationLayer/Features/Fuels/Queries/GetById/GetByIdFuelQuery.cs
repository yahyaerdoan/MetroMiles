using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;

public class GetByIdFuelQuery : IRequest<OperationDataResult<GetByIdFuelResponse>>
{
    public Guid Id { get; set; }

    public class GetByIdFuelQueryHandler(IFuelRepository fuelRepository, IMapper mapper) : IRequestHandler<GetByIdFuelQuery, OperationDataResult<GetByIdFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdFuelResponse>> Handle(GetByIdFuelQuery request, CancellationToken cancellationToken)
        {
            var fuel = await _fuelRepository.GetAsync(predicate: f => f.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = FuelBusinessRules.FuelShouldExistWhenSelected(fuel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdFuelResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdFuelResponse>(existenceCheck.Data));
        }
    }
}
