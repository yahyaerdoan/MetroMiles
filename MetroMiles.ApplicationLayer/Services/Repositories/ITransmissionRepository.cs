using MetroMiles.DomainLayer.Entities;
using Core.PersistenceLayer.Repositories.IRepositories;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface ITransmissionRepository : IAsyncRepository<Transmission, Guid> //,IRepository<Transmission, Guid>
{
}
