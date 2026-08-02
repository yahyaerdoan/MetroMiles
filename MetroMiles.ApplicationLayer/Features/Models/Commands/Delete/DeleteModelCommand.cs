using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Models.Constants;
using MetroMiles.ApplicationLayer.Features.Models.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Delete;

public class DeleteModelCommand : IRequest<OperationDataResult<DeletedModelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [ModelsOperationClaims.Admin, ModelsOperationClaims.Write, ModelsOperationClaims.Delete];

    public class DeleteModelCommandHandler(IModelRepository modelRepository, IMapper mapper, ModelBusinessRules modelBusinessRules)
        : IRequestHandler<DeleteModelCommand, OperationDataResult<DeletedModelResponse>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ModelBusinessRules _modelBusinessRules = modelBusinessRules;

        public async Task<OperationDataResult<DeletedModelResponse>> Handle(DeleteModelCommand request, CancellationToken cancellationToken)
        {
            var existingModel = await _modelRepository.GetAsync(predicate: m => m.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = ModelBusinessRules.ModelShouldExistWhenSelected(existingModel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedModelResponse>();
            }

            var inUseCheck = await _modelBusinessRules.ModelShouldNotBeInUseWhenDeleted(request.Id);
            if (!inUseCheck.IsSuccessful)
            {
                return inUseCheck.ToErrorDataResult<DeletedModelResponse>();
            }

            await _modelRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedModelResponse>(existenceCheck.Data));
        }
    }
}
