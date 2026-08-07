using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace MetroMiles.PersistenceLayer.Repositories;

public class UserOperationClaimRepository(BaseDbContext context) : EfRepositoryBase<UserOperationClaim, int, BaseDbContext>(context), IUserOperationClaimRepository
{
    public async Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId)
    {
        var operationClaims = await Query().AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(o => new OperationClaim { Id = o.OperationClaimId, Name = o.OperationClaim.Name })
            .ToListAsync();
        return operationClaims;
    }
}
