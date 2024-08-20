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

public class EmailAuthenticatorRepository : EfRepositoryBase<EmailAuthenticator, int, BaseDbContext>, IEmailAuthenticatorRepository
{
    public EmailAuthenticatorRepository(BaseDbContext context) : base(context)
    {
    }
}
