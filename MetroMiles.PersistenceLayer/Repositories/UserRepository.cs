using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;

namespace MetroMiles.PersistenceLayer.Repositories;

public class UserRepository(BaseDbContext context) : EfRepositoryBase<User, int, BaseDbContext>(context), IUserRepository
{
}

