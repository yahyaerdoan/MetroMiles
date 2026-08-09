using System.Text.Json.Serialization;
using AutoMapper;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Pipelines.Transactions.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Fuels.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;
using static MetroMiles.ApplicationLayer.Features.Fuels.Constants.FuelsOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;

public class CreateFuelCommand : SecuredRequest<CreatedFuelResponse>, ITransactionAddRequest, ILogAddRequest
{
    public required string Name { get; set; }

    [JsonIgnore]
    public override string[] Roles => [Admin, Write, Add];

    public class CreateFuelCommandHandler(IFuelRepository fuelRepository, IMapper mapper, FuelBusinessRules fuelBusinessRules) : IRequestHandler<CreateFuelCommand, OperationDataResult<CreatedFuelResponse>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly FuelBusinessRules _fuelBusinessRules = fuelBusinessRules;

        public async Task<OperationDataResult<CreatedFuelResponse>> Handle(CreateFuelCommand request, CancellationToken cancellationToken)
        {
            var duplicateCheck = await _fuelBusinessRules.FuelNameCannotBeDuplicatedWhenInserted(request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<CreatedFuelResponse>();
            }

            var fuel = _mapper.Map<Fuel>(request);
            await _fuelRepository.AddAsync(fuel);
            return Result.Success(_mapper.Map<CreatedFuelResponse>(fuel));
        }
    }
}
