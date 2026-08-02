using Core.PersistenceLayer.Repositories.IRepositories;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IModelRepository : IAsyncRepository<Model, Guid> //,IRepository<Model, Guid>
{
}
