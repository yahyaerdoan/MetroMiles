using Core.PersistenceLayer.Repositories.IRepositories;
using Core.SecurityLayer.Entities;

namespace MetroMiles.ApplicationLayer.Services.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, int>//, IRepository<RefreshToken, int>
{
    Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userId, int refreshTokenTTL);
}
