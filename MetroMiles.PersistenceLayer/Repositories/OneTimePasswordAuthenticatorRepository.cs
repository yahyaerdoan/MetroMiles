using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.PersistenceLayer.Repositories;

public class OneTimePasswordAuthenticatorRepository : EfRepositoryBase<OneTimePasswordAuthenticator, int, BaseDbContext>, IOneTimePasswordAuthenticatorRepository
{
    public OneTimePasswordAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }
}
