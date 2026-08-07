using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Models.Constants;
using MetroMiles.ApplicationLayer.Features.Models.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Update;

public class UpdateModelCommand : IRequest<OperationDataResult<UpdatedModelResponse>>, ISecureAddRequest
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid FuelId { get; set; }
    public Guid TransmissionId { get; set; }
    public required string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public required string ImageUrl { get; set; }
    public byte[]? RowVersion { get; set; }
    public string[] Roles => [ModelsOperationClaims.Admin, ModelsOperationClaims.Write, ModelsOperationClaims.Update];

    public class UpdateModelCommandHandler(IModelRepository modelRepository, IMapper mapper, ModelBusinessRules modelBusinessRules)
        : IRequestHandler<UpdateModelCommand, OperationDataResult<UpdatedModelResponse>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ModelBusinessRules _modelBusinessRules = modelBusinessRules;

        public async Task<OperationDataResult<UpdatedModelResponse>> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
        {
            var existingModel = await _modelRepository.GetAsync(predicate: m => m.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = ModelBusinessRules.ModelShouldExistWhenSelected(existingModel);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var versionCheck = ModelBusinessRules.ModelRowVersionShouldMatchWhenUpdated(existenceCheck.Data, request.RowVersion);
            if (!versionCheck.IsSuccessful)
            {
                return versionCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var brandCheck = await _modelBusinessRules.BrandIdShouldExistWhenSelected(request.BrandId);
            if (!brandCheck.IsSuccessful)
            {
                return brandCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var fuelCheck = await _modelBusinessRules.FuelIdShouldExistWhenSelected(request.FuelId);
            if (!fuelCheck.IsSuccessful)
            {
                return fuelCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var transmissionCheck = await _modelBusinessRules.TransmissionIdShouldExistWhenSelected(request.TransmissionId);
            if (!transmissionCheck.IsSuccessful)
            {
                return transmissionCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var duplicateCheck = await _modelBusinessRules.ModelNameCannotBeDuplicatedWhenUpdated(request.Id, request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<UpdatedModelResponse>();
            }

            var model = _mapper.Map(request, existenceCheck.Data);
            await _modelRepository.UpdateAsync(model);
            return Result.Success(_mapper.Map<UpdatedModelResponse>(model));
        }
    }
}
