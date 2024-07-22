using Core.ApplicationLayer.Pipelines.Cachings.Concretions.CacheBehaviors;
using Core.ApplicationLayer.Pipelines.Loggings.Concretions;
using Core.ApplicationLayer.Pipelines.Transactions.Concretions;
using Core.ApplicationLayer.Pipelines.Validations.Concretions;
using Core.CrossCuttingConcernLayer.Loggings.Serilogs.Loggers;
using Core.CrossCuttingConcernLayer.Loggings.Serilogs.Services;
using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.RuleRegistrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Extensions.ServiceRegistrations;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(ValidationAddingBehavior< , >));
            configuration.AddOpenBehavior(typeof(TransactionAddingBehavior< , >));
            configuration.AddOpenBehavior(typeof(CacheAddingBehavior< , >));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior< , >));
            configuration.AddOpenBehavior(typeof(LogAddingBehavior< , >));
        });
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<BaseLoggerService, MsSqlLogger>();
        return services;
    }

    #region Add all BaseBusinessRules classes to service registrations dynamically.
    public static IServiceCollection AddSubClassesOfType(this IServiceCollection services, Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, item);
        return services;
    }
    #endregion
}
