using System.Text.Json.Serialization;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Cachings.Abstractions;
using MediatR;
using ResultHandler.Core.Base;

namespace MetroMiles.ApplicationLayer.Extensions.Requests;

// Server-computed request shape shared by every secured MediatR request regardless of whether it
// carries an Id (Delete/Restore/Update) or not (Create) — Roles is never client input, so it's
// hidden from the request's JSON schema to avoid showing up as a fake editable field in Swagger/Scalar.
public abstract class SecuredRequest<TResponse> : IRequest<OperationDataResult<TResponse>>, ISecureAddRequest
{
    [JsonIgnore]
    public abstract string[] Roles { get; }
}

public abstract class CacheRemovingSecuredRequest<TResponse> : SecuredRequest<TResponse>, ICacheRemoveRequest
{
    [JsonIgnore]
    public string CacheKey => "";

    [JsonIgnore]
    public bool ByPassCache => false;

    [JsonIgnore]
    public abstract string? CacheGroupKey { get; }
}

public abstract class SecuredCommand<TId, TResponse> : SecuredRequest<TResponse>
    where TId : struct
{
    public TId? Id { get; set; }
}

public abstract class CacheRemovingSecuredCommand<TId, TResponse> : CacheRemovingSecuredRequest<TResponse>
    where TId : struct
{
    public TId? Id { get; set; }
}
