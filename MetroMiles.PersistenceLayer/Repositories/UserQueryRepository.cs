using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.PersistenceLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace MetroMiles.PersistenceLayer.Repositories;

public class UserQueryRepository(BaseDbContext context) : IUserQueryRepository
{
    public Task<User?> GetByIdWithDeletedAsync(Guid? id, CancellationToken cancellationToken = default) =>
        context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
}
