using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Transmissions.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

using static MetroMiles.ApplicationLayer.Features.Transmissions.Constants.TransmissionsOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Create;

public class CreateTransmissionCommand : IRequest<OperationDataResult<CreatedTransmissionResponse>>, ISecureAddRequest
{
    public required string Name { get; set; }
    public string[] Roles => [Admin, Write, Add];

    public class CreateTransmissionCommandHandler(ITransmissionRepository transmissionRepository, IMapper mapper, TransmissionBusinessRules transmissionBusinessRules)
        : IRequestHandler<CreateTransmissionCommand, OperationDataResult<CreatedTransmissionResponse>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly TransmissionBusinessRules _transmissionBusinessRules = transmissionBusinessRules;

        public async Task<OperationDataResult<CreatedTransmissionResponse>> Handle(CreateTransmissionCommand request, CancellationToken cancellationToken)
        {
            var duplicateCheck = await _transmissionBusinessRules.TransmissionNameCannotBeDuplicatedWhenInserted(request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<CreatedTransmissionResponse>();
            }

            var transmission = _mapper.Map<Transmission>(request);
            await _transmissionRepository.AddAsync(transmission);
            return Result.Success(_mapper.Map<CreatedTransmissionResponse>(transmission));
        }
    }
}
