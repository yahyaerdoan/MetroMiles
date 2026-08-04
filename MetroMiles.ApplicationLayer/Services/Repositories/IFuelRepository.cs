using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IFuelRepository : IAsyncRepository<Fuel, Guid> //,IRepository<Fuel, Guid>
{
}
