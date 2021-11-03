using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions
{
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class HostExtensions
    {
        /// <summary>
        ///     Registers the game's API within the DI container.
        /// </summary>
        /// <param name="services">The service collection to register the API with.</param>
        /// <param name="api">The game's core API.</param>
        /// <param name="registrar">The registrar to use, to register the API for the current app-side.</param>
        /// <returns>Returns the same instance of <see cref="IServiceResolver" /> that it was passed.</returns>
        public static IServiceCollection RegisterAPI(this IServiceCollection services, ICoreAPI api, IRegistrar registrar)
        {
            registrar.Register(api, services);
            return services;
        }

        /// <summary>
        ///     Registers classes, annotate with <see cref="RegisteredServiceAttribute"/>, within the given assembly.
        /// </summary>
        /// <remarks>
        ///     If no specific type is supplied, the class will be registered via convention:<br/><br/>
        ///
        ///      • If the class implements an interface, the interface will be be used as the representation.<br/>
        ///      • If the class implements more than one interface, the first interface will be be used as the representation.<br/>
        ///      • If the class does not implement an interface, it will be registered as itself.
        /// </remarks>
        /// <param name="services">The service collection to register the services with.</param>
        /// <param name="assembly">The assembly to scan for annotated service classes.</param>
        public static void RegisterAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypesWithAttribute<RegisteredServiceAttribute>()
                .ToList();

            foreach (var (type, attribute) in types)
            {
                var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceScope);
                services.Register(descriptor);
            }
        }
        /// <summary>
        ///     performs custom configuration for the given <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to use.</param>
        /// <param name="serviceRegistrationFactory">A factory method, allowing custom service registration.</param>
        /// <returns>Returns the same instance of <see cref="IServiceCollection" /> that it was passed.</returns>
        public static IServiceCollection Configure(this IServiceCollection services, Action<IServiceCollection> serviceRegistrationFactory)
        {
            serviceRegistrationFactory(services);
            return services;
        }
    }
}