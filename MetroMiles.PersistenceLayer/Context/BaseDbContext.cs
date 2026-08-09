using System.Reflection;
using Core.SecurityLayer.Identity;
using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MetroMiles.PersistenceLayer.Context;

public class BaseDbContext : BaseIdentityDbContext<User, Role, Guid>
{
    public IConfiguration Configuration { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Car> Cars { get; set; }

    public DbSet<Fuel> Fuels { get; set; }

    public DbSet<Model> Models { get; set; }

    public DbSet<Transmission> Transmissions { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
