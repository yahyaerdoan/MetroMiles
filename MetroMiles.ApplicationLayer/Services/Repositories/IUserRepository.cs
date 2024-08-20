using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IUserRepository : IAsyncRepository<User, int>//, IRepository<User, int>
{
}
