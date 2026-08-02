using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;

public class RestoreCarCommand : IRequest<OperationDataResult<RestoredCarResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [CarsOperationClaims.Admin, CarsOperationClaims.Write, CarsOperationClaims.Update];

    public class RestoreCarCommandHandler(ICarRepository carRepository, IMapper mapper) : IRequestHandler<RestoreCarCommand, OperationDataResult<RestoredCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<RestoredCarResponse>> Handle(RestoreCarCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetAsync(predicate: c => c.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldBeDeletedWhenRestored(existingCar);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredCarResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _carRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredCarResponse>(existenceCheck.Data));
        }
    }
}
