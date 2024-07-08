﻿using Core.PersistenceLayer.Repositories.EfRepositories;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.PersistenceLayer.Context;

namespace MetroMiles.PersistenceLayer.Repositories;

public class FuelRepository : EfRepositoryBase<Fuel, Guid, BaseDbContext>, IFuelRepository
{
    public FuelRepository(BaseDbContext context) : base(context)
    {
    }
}
