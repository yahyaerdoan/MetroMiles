using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace MetroMiles.PersistenceLayer.Repositories;

public class RefreshTokenRepository(BaseDbContext context) : EfRepositoryBase<RefreshToken, int, BaseDbContext>(context), IRefreshTokenRepository
{
    public async Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userId, int refreshTokenTTL)
    {
        var refreshTokens = await Query().AsNoTracking()
            .Where(
            r => r.UserId == userId
            && r.Revoked == null
            && r.Expires >= DateTime.UtcNow
            && r.CreatedDate.AddDays(refreshTokenTTL) <= DateTime.UtcNow).ToListAsync();
        return refreshTokens;
    }
}
