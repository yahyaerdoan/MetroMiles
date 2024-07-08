using MetroMiles.DomainLayer.Entities;
using Core.PersistenceLayer.Repositories.IRepositories;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IModelRepository : IAsyncRepository<Model, Guid> //,IRepository<Model, Guid>
{
}
