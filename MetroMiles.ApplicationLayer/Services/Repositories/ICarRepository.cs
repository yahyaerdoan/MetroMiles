using MetroMiles.DomainLayer.Entities;
using Core.PersistenceLayer.Repositories.IRepositories;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface ICarRepository : IAsyncRepository<Car, Guid> //,IRepository<Car, Guid>
{
}
