using MetroMiles.DomainLayer.Entities;
using Core.PersistenceLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.PersistenceLayer.Repositories.IRepositories;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IBrandRepository : IAsyncRepository<Brand,Guid> //,IRepository<Brand, Guid>
{
}
