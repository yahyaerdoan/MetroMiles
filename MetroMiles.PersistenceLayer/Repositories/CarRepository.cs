using Core.PersistenceLayer.Repositories.EfRepositories;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.PersistenceLayer.Context;

namespace MetroMiles.PersistenceLayer.Repositories;

public class CarRepository : EfRepositoryBase<Car, Guid, BaseDbContext>, ICarRepository
{
    public CarRepository(BaseDbContext context) : base(context)
    {
    }
}
