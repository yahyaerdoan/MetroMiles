using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface ICarRepository : IAsyncRepository<Car, Guid>//,IRepository<Car, Guid>
{
}
