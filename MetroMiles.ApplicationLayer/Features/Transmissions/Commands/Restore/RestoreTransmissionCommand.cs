using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Features.Transmissions.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Restore;

public class RestoreTransmissionCommand : SecuredCommand<Guid, RestoredTransmissionResponse>
{
    public override string[] Roles => TransmissionsOperationClaims.UpdateRoles;

    public class RestoreTransmissionCommandHandler(ITransmissionRepository transmissionRepository, IMapper mapper, TransmissionBusinessRules transmissionBusinessRules)
        : IRequestHandler<RestoreTransmissionCommand, OperationDataResult<RestoredTransmissionResponse>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly TransmissionBusinessRules _transmissionBusinessRules = transmissionBusinessRules;

        public async Task<OperationDataResult<RestoredTransmissionResponse>> Handle(RestoreTransmissionCommand request, CancellationToken cancellationToken)
        {
            var existingTransmission = await _transmissionRepository.GetAsync(predicate: t => t.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = TransmissionBusinessRules.TransmissionShouldBeDeletedWhenRestored(existingTransmission);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredTransmissionResponse>();
            }

            var duplicateCheck = await _transmissionBusinessRules.TransmissionNameCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, existenceCheck.Data.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<RestoredTransmissionResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _transmissionRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredTransmissionResponse>(existenceCheck.Data));
        }
    }
}
