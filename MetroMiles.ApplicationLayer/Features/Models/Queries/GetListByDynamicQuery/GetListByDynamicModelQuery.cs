using AutoMapper;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Dynamics.Dynamic;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;

public class GetListByDynamicModelQuery : IRequest<OperationDataResult<GetListResponse<GetListByDynamicModelListItemDto>>>
{
    public required PageRequest PageRequest { get; set; }
    public DynamicQuery? DynamicQuery { get; set; }

    public class GetListByDynamicModelQueryHandler(IModelRepository modelRepository, IMapper mapper) : IRequestHandler<GetListByDynamicModelQuery, OperationDataResult<GetListResponse<GetListByDynamicModelListItemDto>>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListByDynamicModelListItemDto>>> Handle(GetListByDynamicModelQuery request, CancellationToken cancellationToken)
        {
            var models = await _modelRepository.GetListByDynamicAsync(
                 request.DynamicQuery ?? new DynamicQuery(),
                 include: m => m.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission!),
                 index: request.PageRequest.PageIndex,
                 size: request.PageRequest.PageSize,
                 cancellationToken: cancellationToken
                 );
            var response = _mapper.Map<GetListResponse<GetListByDynamicModelListItemDto>>(models);
            return Result.Success(response);
        }
    }
}
