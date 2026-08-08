using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using MediatR;
using ResultHandler.Core.Base;

namespace MetroMiles.ApplicationLayer.Extensions.Requests;

public abstract class SecuredCommand<TId, TResponse> : IRequest<OperationDataResult<TResponse>>, ISecureAddRequest
    where TId : struct
{
    public TId? Id { get; set; }
    public abstract string[] Roles { get; }
}

public abstract class CacheRemovingSecuredCommand<TId, TResponse> : SecuredCommand<TId, TResponse>, ICacheRemoveRequest
    where TId : struct
{
    public string CacheKey => "";
    public bool ByPassCache => false;
    public abstract string? CacheGroupKey { get; }
}
