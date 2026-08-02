using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Models.Constants;
using MetroMiles.ApplicationLayer.Features.Models.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Restore;

public class RestoreModelCommand : IRequest<OperationDataResult<RestoredModelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [ModelsOperationClaims.Admin, ModelsOperationClaims.Write, ModelsOperationClaims.Update];

    public class RestoreModelCommandHandler(IModelRepository modelRepository, IMapper mapper, ModelBusinessRules modelBusinessRules)
        : IRequestHandler<RestoreModelCommand, OperationDataResult<RestoredModelResponse>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ModelBusinessRules _modelBusinessRules = modelBusinessRules;

        public async Task<OperationDataResult<RestoredModelResponse>> Handle(RestoreModelCommand request, CancellationToken cancellationToken)
        {
            var existingModel = await _modelRepository.GetAsync(predicate: m => m.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = ModelBusinessRules.ModelShouldBeDeletedWhenRestored(existingModel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredModelResponse>();
            }

            var duplicateCheck = await _modelBusinessRules.ModelNameCannotBeDuplicatedWhenUpdated(existenceCheck.Data.Id, existenceCheck.Data.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<RestoredModelResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _modelRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredModelResponse>(existenceCheck.Data));
        }
    }
}
