using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Models.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;

public class GetByIdModelQuery : IRequest<OperationDataResult<GetByIdModelResponse>>
{
    public Guid Id { get; set; }

    public class GetByIdModelQueryHandler(IModelRepository modelRepository, IMapper mapper) : IRequestHandler<GetByIdModelQuery, OperationDataResult<GetByIdModelResponse>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdModelResponse>> Handle(GetByIdModelQuery request, CancellationToken cancellationToken)
        {
            var model = await _modelRepository.GetAsync(
                predicate: m => m.Id == request.Id,
                include: q => q.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission!),
                cancellationToken: cancellationToken);
            var existenceCheck = ModelBusinessRules.ModelShouldExistWhenSelected(model);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdModelResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdModelResponse>(existenceCheck.Data));
        }
    }
}
