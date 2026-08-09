using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdateFuelCommand : SecuredCommand<Guid, UpdatedFuelResponse>
{
    public required string Name { get; set; }

    public byte[]? RowVersion { get; set; }

    [JsonIgnore]
    public override string[] Roles => FuelsOperationClaims.UpdateRoles;

    public class UpdateFuelCommandHandler(IFuelRepository fuelRepository, IMapper mapper, FuelBusinessRules fuelBusinessRules) : IRequestHandler<UpdateFuelCommand, OperationDataResult<UpdatedFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly FuelBusinessRules _fuelBusinessRules = fuelBusinessRules;

        public async Task<OperationDataResult<UpdatedFuelResponse>> Handle(UpdateFuelCommand request, CancellationToken cancellationToken)
        {
            var existingFuel = await _fuelRepository.GetAsync(predicate: f => f.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = FuelBusinessRules.FuelShouldExistWhenSelected(existingFuel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedFuelResponse>();
            }

            var versionCheck = FuelBusinessRules.FuelRowVersionShouldMatchWhenUpdated(existenceCheck.Data, request.RowVersion);
            if (!versionCheck.IsSuccessful)
            {
                return versionCheck.ToErrorDataResult<UpdatedFuelResponse>();
            }

            var duplicateCheck = await _fuelBusinessRules.FuelNameCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<UpdatedFuelResponse>();
            }

            var fuel = _mapper.Map(request, existenceCheck.Data);
            await _fuelRepository.UpdateAsync(fuel);
            return Result.Success(_mapper.Map<UpdatedFuelResponse>(fuel));
        }
    }
}
