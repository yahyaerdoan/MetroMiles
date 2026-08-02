using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;

public class RestoreFuelCommand : IRequest<OperationDataResult<RestoredFuelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [FuelsOperationClaims.Admin, FuelsOperationClaims.Write, FuelsOperationClaims.Update];

    public class RestoreFuelCommandHandler(IFuelRepository fuelRepository, IMapper mapper) : IRequestHandler<RestoreFuelCommand, OperationDataResult<RestoredFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<RestoredFuelResponse>> Handle(RestoreFuelCommand request, CancellationToken cancellationToken)
        {
            var existingFuel = await _fuelRepository.GetAsync(predicate: f => f.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = FuelBusinessRules.FuelShouldBeDeletedWhenRestored(existingFuel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredFuelResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _fuelRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredFuelResponse>(existenceCheck.Data));
        }
    }
}
