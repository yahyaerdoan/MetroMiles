using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface ITransmissionRepository : IAsyncRepository<Transmission, Guid> //,IRepository<Transmission, Guid>
{
}
