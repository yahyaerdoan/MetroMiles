using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using MetroMiles.ApplicationLayer.Features.Models.Constants;
using MetroMiles.DomainLayer.Entities;

using ResultHandler.Core.Abstractions;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Models.Rules;

public class ModelBusinessRules : BaseBusinessRules
{
    public static IOperationResult<Model> ModelShouldExistWhenSelected(Model? model)
        => model is null ? Result.NotFound<Model>(ModelMessages.ModelNotExists) : Result.Success(model);
}
