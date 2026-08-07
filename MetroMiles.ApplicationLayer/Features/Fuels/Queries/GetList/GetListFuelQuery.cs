using AutoMapper;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;

public class GetListFuelQuery : IRequest<OperationDataResult<GetListResponse<GetListFuelListItemDto>>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListFuelQueryHandler(IFuelRepository fuelRepository, IMapper mapper) : IRequestHandler<GetListFuelQuery, OperationDataResult<GetListResponse<GetListFuelListItemDto>>>
    {
        private readonly IFuelRepository _fuelRepository = fuelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListFuelListItemDto>>> Handle(GetListFuelQuery request, CancellationToken cancellationToken)
        {
            var fuels = await _fuelRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListFuelListItemDto>>(fuels);
            return Result.Success(response);
        }
    }
}
