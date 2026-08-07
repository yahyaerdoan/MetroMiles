using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Pipelines.Transactions.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Models.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;
using static MetroMiles.ApplicationLayer.Features.Models.Constants.ModelsOperationClaims;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Create;

public class CreateModelCommand : IRequest<OperationDataResult<CreatedModelResponse>>, ITransactionAddRequest, ILogAddRequest, ISecureAddRequest
{
    public Guid BrandId { get; set; }
    public Guid FuelId { get; set; }
    public Guid TransmissionId { get; set; }
    public required string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public required string ImageUrl { get; set; }
    public string[] Roles => [Admin, Write, Add];

    public class CreateModelCommandHandler(IModelRepository modelRepository, IMapper mapper, ModelBusinessRules modelBusinessRules)
        : IRequestHandler<CreateModelCommand, OperationDataResult<CreatedModelResponse>>
    {
        private readonly IModelRepository _modelRepository = modelRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ModelBusinessRules _modelBusinessRules = modelBusinessRules;

        public async Task<OperationDataResult<CreatedModelResponse>> Handle(CreateModelCommand request, CancellationToken cancellationToken)
        {
            var brandCheck = await _modelBusinessRules.BrandIdShouldExistWhenSelected(request.BrandId);
            if (!brandCheck.IsSuccessful)
            {
                return brandCheck.ToErrorDataResult<CreatedModelResponse>();
            }

            var fuelCheck = await _modelBusinessRules.FuelIdShouldExistWhenSelected(request.FuelId);
            if (!fuelCheck.IsSuccessful)
            {
                return fuelCheck.ToErrorDataResult<CreatedModelResponse>();
            }

            var transmissionCheck = await _modelBusinessRules.TransmissionIdShouldExistWhenSelected(request.TransmissionId);
            if (!transmissionCheck.IsSuccessful)
            {
                return transmissionCheck.ToErrorDataResult<CreatedModelResponse>();
            }

            var duplicateCheck = await _modelBusinessRules.ModelNameCannotBeDuplicatedWhenInserted(request.Name);
            if (!duplicateCheck.IsSuccessful)
            {
                return duplicateCheck.ToErrorDataResult<CreatedModelResponse>();
            }

            var model = _mapper.Map<Model>(request);
            await _modelRepository.AddAsync(model);
            return Result.Success(_mapper.Map<CreatedModelResponse>(model));
        }
    }
}
