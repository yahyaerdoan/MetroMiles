using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;

public class GetByIdCarQuery : IRequest<OperationDataResult<GetByIdCarResponse>>
{
    public Guid Id { get; set; }

    public class GetByIdCarQueryHandler(ICarRepository carRepository, IMapper mapper) : IRequestHandler<GetByIdCarQuery, OperationDataResult<GetByIdCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdCarResponse>> Handle(GetByIdCarQuery request, CancellationToken cancellationToken)
        {
            var car = await _carRepository.GetAsync(
                predicate: c => c.Id == request.Id,
                include: q => q.Include(c => c.Model!).ThenInclude(m => m.Brand!),
                cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldExistWhenSelected(car);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdCarResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdCarResponse>(existenceCheck.Data));
        }
    }
}
