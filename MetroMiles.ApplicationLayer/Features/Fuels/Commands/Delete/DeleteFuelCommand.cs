using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;

public class DeleteFuelCommand : IRequest<OperationDataResult<DeletedFuelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [FuelsOperationClaims.Admin, FuelsOperationClaims.Write, FuelsOperationClaims.Delete];

    public class DeleteFuelCommandHandler(IFuelRepository fuelRepository, IMapper mapper, FuelBusinessRules fuelBusinessRules) : IRequestHandler<DeleteFuelCommand, OperationDataResult<DeletedFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly FuelBusinessRules _fuelBusinessRules = fuelBusinessRules;

        public async Task<OperationDataResult<DeletedFuelResponse>> Handle(DeleteFuelCommand request, CancellationToken cancellationToken)
        {
            var existingFuel = await _fuelRepository.GetAsync(predicate: f => f.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = FuelBusinessRules.FuelShouldExistWhenSelected(existingFuel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedFuelResponse>();
            }

            var inUseCheck = await _fuelBusinessRules.FuelShouldNotBeInUseWhenDeleted(request.Id);
            if (!inUseCheck.IsSuccessful)
            {
                return inUseCheck.ToErrorDataResult<DeletedFuelResponse>();
            }

            await _fuelRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedFuelResponse>(existenceCheck.Data));
        }
    }
}
