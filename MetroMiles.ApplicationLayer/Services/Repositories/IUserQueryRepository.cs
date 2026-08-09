using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

// UserManager<User> covers standard Identity operations (create/update/find/check password), but it
// has no notion of MetroMiles' soft-delete query filter, so it can never see a deleted user. This is
// the one narrow lookup Restore genuinely needs beyond what UserManager exposes.
public interface IUserQueryRepository
{
    Task<User?> GetByIdWithDeletedAsync(Guid? id, CancellationToken cancellationToken = default);
}
