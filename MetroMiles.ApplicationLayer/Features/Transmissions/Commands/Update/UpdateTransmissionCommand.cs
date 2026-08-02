using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Features.Transmissions.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;

public class UpdateTransmissionCommand : IRequest<OperationDataResult<UpdatedTransmissionResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string[] Roles => [TransmissionsOperationClaims.Admin, TransmissionsOperationClaims.Write, TransmissionsOperationClaims.Update];

    public class UpdateTransmissionCommandHandler(ITransmissionRepository transmissionRepository, IMapper mapper)
        : IRequestHandler<UpdateTransmissionCommand, OperationDataResult<UpdatedTransmissionResponse>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<UpdatedTransmissionResponse>> Handle(UpdateTransmissionCommand request, CancellationToken cancellationToken)
        {
            var existingTransmission = await _transmissionRepository.GetAsync(predicate: t => t.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = TransmissionBusinessRules.TransmissionShouldExistWhenSelected(existingTransmission);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedTransmissionResponse>();
            }

            var transmission = _mapper.Map(request, existenceCheck.Data);
            await _transmissionRepository.UpdateAsync(transmission);
            return Result.Success(_mapper.Map<UpdatedTransmissionResponse>(transmission));
        }
    }
}
