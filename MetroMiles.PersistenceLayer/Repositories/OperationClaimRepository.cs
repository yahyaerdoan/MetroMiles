using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;

namespace MetroMiles.PersistenceLayer.Repositories;

public class OperationClaimRepository(BaseDbContext context) : EfRepositoryBase<OperationClaim, int, BaseDbContext>(context), IOperationClaimRepository
{
}
