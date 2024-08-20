using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.PersistenceLayer.Repositories;

public class UserOperationClaimRepository : EfRepositoryBase<UserOperationClaim, int, BaseDbContext>, IUserOperationClaimRepository
{
    public UserOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IList<OperationClaim>> GetOperationClaimsByUserIdAsync(int userId)
    {
        var operationClaims = await Query().AsNoTracking()
            .Where(u => u.UserId == userId)
            .Select(o => new OperationClaim { Id = o.OperationClaimId, Name = o.OperationClaim.Name })
            .ToListAsync();
        return operationClaims;
    }
}
