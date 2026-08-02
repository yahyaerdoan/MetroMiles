using AutoMapper;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;

public class GetListBrandQuery : IRequest<OperationDataResult<GetListResponse<GetListBrandListItemDto>>>, ICacheAddRequest, ILogAddRequest
{
    #region GetListBrandQuery & ICacheAddRequest Properties
    public required PageRequest PageRequest { get; set; }
    public string CacheKey => $"GetListBrandQuery({PageRequest.PageSize},{PageRequest.PageIndex})";
    public bool ByPassCache { get; }
    public TimeSpan? SlidingExpiration { get; }
    public string? CacheGroupKey => "GetBrands";
    #endregion

    public class GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper) : IRequestHandler<GetListBrandQuery, OperationDataResult<GetListResponse<GetListBrandListItemDto>>>
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListBrandListItemDto>>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListBrandListItemDto>>(brands);
            return Result.Success(response);
        }
    }
}
