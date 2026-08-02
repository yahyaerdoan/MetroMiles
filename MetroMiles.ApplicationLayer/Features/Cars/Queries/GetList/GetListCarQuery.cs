using AutoMapper;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetList;

public class GetListCarQuery : IRequest<OperationDataResult<GetListResponse<GetListCarListItemDto>>>
{
    public required PageRequest PageRequest { get; set; }

    public class GetListCarQueryHandler(ICarRepository carRepository, IMapper mapper) : IRequestHandler<GetListCarQuery, OperationDataResult<GetListResponse<GetListCarListItemDto>>>
    {
        private readonly ICarRepository _carRepository = carRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListCarListItemDto>>> Handle(GetListCarQuery request, CancellationToken cancellationToken)
        {
            var cars = await _carRepository.GetListAsync(
                include: c => c.Include(c => c.Model!).ThenInclude(m => m.Brand!),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListCarListItemDto>>(cars);
            return Result.Success(response);
        }
    }
}
