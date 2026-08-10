using AutoMapper;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;

public class GetListTransmissionQuery : IRequest<OperationDataResult<GetListResponse<GetListTransmissionListItemResponse>>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListTransmissionQueryHandler(ITransmissionRepository transmissionRepository, IMapper mapper) : IRequestHandler<GetListTransmissionQuery, OperationDataResult<GetListResponse<GetListTransmissionListItemResponse>>>
    {
        private readonly ITransmissionRepository _transmissionRepository = transmissionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListTransmissionListItemResponse>>> Handle(GetListTransmissionQuery request, CancellationToken cancellationToken)
        {
            var transmissions = await _transmissionRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListTransmissionListItemResponse>>(transmissions);
            return Result.Success(response);
        }
    }
}
