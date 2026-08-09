using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;

public class RestoreCarCommand : SecuredCommand<Guid, RestoredCarResponse>
{
    [JsonIgnore]
    public override string[] Roles => CarsOperationClaims.UpdateRoles;

    public class RestoreCarCommandHandler(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules) : IRequestHandler<RestoreCarCommand, OperationDataResult<RestoredCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;
        private readonly CarBusinessRules _carBusinessRules = carBusinessRules;

        public async Task<OperationDataResult<RestoredCarResponse>> Handle(RestoreCarCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetAsync(predicate: c => c.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldBeDeletedWhenRestored(existingCar);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredCarResponse>();
            }

            var duplicateCheck = await _carBusinessRules.PlateCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, existenceCheck.Data.Plate);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<RestoredCarResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _carRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredCarResponse>(existenceCheck.Data));
        }
    }
}
