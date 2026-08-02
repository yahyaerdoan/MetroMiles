using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IUserRepository : IAsyncRepository<User, int>//, IRepository<User, int>
{
}
