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

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;

public class RestoreFuelCommand : SecuredCommand<Guid, RestoredFuelResponse>
{
    [JsonIgnore]
    public override string[] Roles => FuelsOperationClaims.UpdateRoles;

    public class RestoreFuelCommandHandler(IFuelRepository fuelRepository, IMapper mapper, FuelBusinessRules fuelBusinessRules) : IRequestHandler<RestoreFuelCommand, OperationDataResult<RestoredFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly FuelBusinessRules _fuelBusinessRules = fuelBusinessRules;

        public async Task<OperationDataResult<RestoredFuelResponse>> Handle(RestoreFuelCommand request, CancellationToken cancellationToken)
        {
            var existingFuel = await _fuelRepository.GetAsync(predicate: f => f.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = FuelBusinessRules.FuelShouldBeDeletedWhenRestored(existingFuel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredFuelResponse>();
            }

            var duplicateCheck = await _fuelBusinessRules.FuelNameCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, existenceCheck.Data.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<RestoredFuelResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _fuelRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredFuelResponse>(existenceCheck.Data));
        }
    }
}
