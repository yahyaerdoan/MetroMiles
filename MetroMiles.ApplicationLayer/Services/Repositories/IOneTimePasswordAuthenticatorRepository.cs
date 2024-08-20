using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IOneTimePasswordAuthenticatorRepository : IAsyncRepository<OneTimePasswordAuthenticator, int>//, IRepository<OneTimePasswordAuthenticator, int>
{
}
