using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Features.Cars.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities.Enums;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Update;

public class UpdateCarCommand : SecuredCommand<Guid, UpdatedCarResponse>
{
    public Guid? ModelId { get; set; }

    public int Kilometer { get; set; }

    public int Mile { get; set; }

    public short ModelYear { get; set; }

    public required string Plate { get; set; }

    public short MinFindexScore { get; set; }

    public CarStatus Status { get; set; }

    public byte[]? RowVersion { get; set; }

    [JsonIgnore]
    public override string[] Roles => CarsOperationClaims.UpdateRoles;

    public class UpdateCarCommandHandler(ICarRepository carRepository, IMapper mapper, CarBusinessRules carBusinessRules) : IRequestHandler<UpdateCarCommand, OperationDataResult<UpdatedCarResponse>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;
        private readonly CarBusinessRules _carBusinessRules = carBusinessRules;

        public async Task<OperationDataResult<UpdatedCarResponse>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            var existingCar = await _carRepository.GetAsync(predicate: c => c.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = CarBusinessRules.CarShouldExistWhenSelected(existingCar);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedCarResponse>();
            }

            var versionCheck = CarBusinessRules.CarRowVersionShouldMatchWhenUpdated(existenceCheck.Data, request.RowVersion);
            if (!versionCheck.IsSuccessful)
            {
                return versionCheck.ToErrorDataResult<UpdatedCarResponse>();
            }

            var modelCheck = await _carBusinessRules.ModelIdShouldExistWhenSelected(request.ModelId);
            if (!modelCheck.IsSuccessful)
            {
                return modelCheck.ToErrorDataResult<UpdatedCarResponse>();
            }

            var plateCheck = await _carBusinessRules.PlateCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, request.Plate);
            if (!plateCheck.IsSuccessful)
            {
                return plateCheck.ToErrorDataResult<UpdatedCarResponse>();
            }

            var rentedGuardCheck = CarBusinessRules.PlateAndModelCannotChangeWhileRented(existenceCheck.Data, request.Plate, request.ModelId);
            if (!rentedGuardCheck.IsSuccessful)
            {
                return rentedGuardCheck.ToErrorDataResult<UpdatedCarResponse>();
            }

            var car = _mapper.Map(request, existenceCheck.Data);
            await _carRepository.UpdateAsync(car);
            return Result.Success(_mapper.Map<UpdatedCarResponse>(car));
        }
    }
}
