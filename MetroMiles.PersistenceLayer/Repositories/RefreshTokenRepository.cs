using Core.PersistenceLayer.Repositories.EfRepositories;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.PersistenceLayer.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, int, BaseDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<List<RefreshToken>> GetOldRefreshTokensAsync(int userId, int refreshTokenTTL)
    {
        List<RefreshToken> refreshTokens = await Query().AsNoTracking()
            .Where(
            r => r.UserId == userId 
            && r.Revoked == null 
            && r.Expires >= DateTime.UtcNow 
            && r.CreatedDate.AddDays(refreshTokenTTL) <= DateTime.UtcNow).ToListAsync();
        return refreshTokens;
    }
}
