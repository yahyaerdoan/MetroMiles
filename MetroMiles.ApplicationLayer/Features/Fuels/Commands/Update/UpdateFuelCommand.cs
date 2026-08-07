using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdateFuelCommand : IRequest<OperationDataResult<UpdatedFuelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public byte[]? RowVersion { get; set; }
    public string[] Roles => [FuelsOperationClaims.Admin, FuelsOperationClaims.Write, FuelsOperationClaims.Update];

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

            var duplicateCheck = await _fuelBusinessRules.FuelNameCannotBeDuplicatedWhenUpdated(request.Id, request.Name);
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
