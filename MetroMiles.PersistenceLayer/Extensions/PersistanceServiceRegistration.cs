using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.PersistenceLayer.Context;
using MetroMiles.PersistenceLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.PersistenceLayer.Extensions
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<BaseDbContext>(options => options.UseInMemoryDatabase("DatabaseName"));
            services.AddDbContext<BaseDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("FakeDatabaseName")));
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
            services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
            services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
            services.AddScoped<IOneTimePasswordAuthenticatorRepository, OneTimePasswordAuthenticatorRepository>(); 
            return services;
        }
    }
}
