using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IUserOperationClaimRepository : IAsyncRepository<UserOperationClaim, int>//, IRepository<UserOperationClaim, int>
{
    Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId);
}
