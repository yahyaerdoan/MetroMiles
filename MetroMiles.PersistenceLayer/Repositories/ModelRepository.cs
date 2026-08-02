using Core.PersistenceLayer.Repositories.EfRepositories;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.PersistenceLayer.Context;

namespace MetroMiles.PersistenceLayer.Repositories;

public class ModelRepository(BaseDbContext context) : EfRepositoryBase<Model, Guid, BaseDbContext>(context), IModelRepository
{
}
