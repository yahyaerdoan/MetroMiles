using MetroMiles.DomainLayer.Entities;
using Core.PersistenceLayer.Repositories.IRepositories;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IFuelRepository : IAsyncRepository<Fuel, Guid> //,IRepository<Fuel, Guid>
{
}
