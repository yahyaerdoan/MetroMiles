using Core.SecurityLayer.Extensions;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using MetroMiles.PersistenceLayer.Context;
using MetroMiles.PersistenceLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MetroMiles.PersistenceLayer.Extensions;

public static class PersistanceServiceRegistration
{
    public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("DatabaseName"));
        services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(configuration.GetRequiredConnectionString("FakeDatabaseName")));
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IModelRepository, ModelRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IFuelRepository, FuelRepository>();
        services.AddScoped<ITransmissionRepository, TransmissionRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddCoreIdentity<User, Role, Guid, BaseDbContext>();
        return services;
    }
}
