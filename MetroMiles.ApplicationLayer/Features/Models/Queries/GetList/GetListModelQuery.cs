using AutoMapper;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;

public class GetListModelQuery : IRequest<OperationDataResult<GetListResponse<GetListModelListItemResponse>>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListModelQueryHandler(IModelRepository modelRepository, IMapper mapper) : IRequestHandler<GetListModelQuery, OperationDataResult<GetListResponse<GetListModelListItemResponse>>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListModelListItemResponse>>> Handle(GetListModelQuery request, CancellationToken cancellationToken)
        {
            var models = await _modelRepository.GetListAsync(
                include: m => m.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission!),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken
                );
            var response = _mapper.Map<GetListResponse<GetListModelListItemResponse>>(models);
            return Result.Success(response);
        }
    }
}
