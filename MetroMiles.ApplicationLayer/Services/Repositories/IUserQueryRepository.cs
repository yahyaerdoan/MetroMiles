using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

// UserManager<User> can't bypass the soft-delete query filter — needed for Restore.
public interface IUserQueryRepository
{
    Task<User?> GetByIdWithDeletedAsync(Guid? id, CancellationToken cancellationToken = default);
}
