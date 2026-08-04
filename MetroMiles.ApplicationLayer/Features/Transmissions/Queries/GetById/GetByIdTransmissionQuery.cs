using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Transmissions.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;

public class GetByIdTransmissionQuery : IRequest<OperationDataResult<GetByIdTransmissionResponse>>
{
    public Guid Id { get; set; }

    public class GetByIdTransmissionQueryHandler(ITransmissionRepository transmissionRepository, IMapper mapper) : IRequestHandler<GetByIdTransmissionQuery, OperationDataResult<GetByIdTransmissionResponse>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdTransmissionResponse>> Handle(GetByIdTransmissionQuery request, CancellationToken cancellationToken)
        {
            var transmission = await _transmissionRepository.GetAsync(predicate: t => t.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = TransmissionBusinessRules.TransmissionShouldExistWhenSelected(transmission);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdTransmissionResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdTransmissionResponse>(existenceCheck.Data));
        }
    }
}
