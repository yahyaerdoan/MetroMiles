using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IBrandRepository : IAsyncRepository<Brand, Guid> //,IRepository<Brand, Guid>
{
}
