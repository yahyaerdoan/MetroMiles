using System.Text.Json.Serialization;
using AutoMapper;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;

public class GetListBrandQuery : IRequest<OperationDataResult<GetListResponse<GetListBrandListItemDto>>>, ICacheAddRequest, ILogAddRequest
{
    // GetListBrandQuery & ICacheAddRequest Properties
    public required PageRequest PageRequest { get; set; }

    [JsonIgnore]
    public string CacheKey => $"GetListBrandQuery({PageRequest.PageSize},{PageRequest.PageIndex})";

    [JsonIgnore]
    public bool ByPassCache { get; }

    [JsonIgnore]
    public TimeSpan? SlidingExpiration { get; }

    [JsonIgnore]
    public string? CacheGroupKey => "GetBrands";

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
