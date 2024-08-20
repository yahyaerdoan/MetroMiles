using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IEmailAuthenticatorRepository : IAsyncRepository<EmailAuthenticator, int>//, IRepository<EmailAuthenticator, int>
{
}
