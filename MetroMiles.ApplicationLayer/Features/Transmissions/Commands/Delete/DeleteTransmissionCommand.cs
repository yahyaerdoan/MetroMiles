using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Features.Transmissions.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Delete;

public class DeleteTransmissionCommand : IRequest<OperationDataResult<DeletedTransmissionResponse>>, ISecureAddRequest
{
    public Guid? Id { get; set; }
    public string[] Roles => [TransmissionsOperationClaims.Admin, TransmissionsOperationClaims.Write, TransmissionsOperationClaims.Delete];

    public class DeleteTransmissionCommandHandler(ITransmissionRepository transmissionRepository, IMapper mapper, TransmissionBusinessRules transmissionBusinessRules)
        : IRequestHandler<DeleteTransmissionCommand, OperationDataResult<DeletedTransmissionResponse>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly TransmissionBusinessRules _transmissionBusinessRules = transmissionBusinessRules;

        public async Task<OperationDataResult<DeletedTransmissionResponse>> Handle(DeleteTransmissionCommand request, CancellationToken cancellationToken)
        {
            var existingTransmission = await _transmissionRepository.GetAsync(predicate: t => t.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = TransmissionBusinessRules.TransmissionShouldExistWhenSelected(existingTransmission);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedTransmissionResponse>();
            }

            var inUseCheck = await _transmissionBusinessRules.TransmissionShouldNotBeInUseWhenDeleted(existenceCheck.Data.Id);
            if (!inUseCheck.IsSuccessful)
            {
                return inUseCheck.ToErrorDataResult<DeletedTransmissionResponse>();
            }

            await _transmissionRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedTransmissionResponse>(existenceCheck.Data));
        }
    }
}
