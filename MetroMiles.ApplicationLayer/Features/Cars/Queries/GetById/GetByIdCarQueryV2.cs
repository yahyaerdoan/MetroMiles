using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;

public class GetByIdCarQueryV2 : IRequest<OperationDataResult<GetByIdCarResponseV2>>
{
    public Guid Id { get; set; }

    public class GetByIdCarQueryV2Handler(ICarRepository carRepository, IMapper mapper) : IRequestHandler<GetByIdCarQueryV2, OperationDataResult<GetByIdCarResponseV2>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdCarResponseV2>> Handle(GetByIdCarQueryV2 request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetAsync(
                predicate: c => c.Id == request.Id,
                include: q => q.Include(c => c.Model!).ThenInclude(m => m.Brand!),
                cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldExistWhenSelected(car);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdCarResponseV2>();
            }

            return Result.Success(_mapper.Map<GetByIdCarResponseV2>(existenceCheck.Data));
        }
    }
}
