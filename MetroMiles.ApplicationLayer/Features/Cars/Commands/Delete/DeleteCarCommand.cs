using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Delete;

public class DeleteCarCommand : IRequest<OperationDataResult<DeletedCarResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [CarsOperationClaims.Admin, CarsOperationClaims.Write, CarsOperationClaims.Delete];

    public class DeleteCarCommandHandler(ICarRepository carRepository, IMapper mapper) : IRequestHandler<DeleteCarCommand, OperationDataResult<DeletedCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<DeletedCarResponse>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetAsync(predicate: c => c.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldExistWhenSelected(existingCar);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedCarResponse>();
            }

            await _carRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedCarResponse>(existenceCheck.Data));
        }
    }
}
