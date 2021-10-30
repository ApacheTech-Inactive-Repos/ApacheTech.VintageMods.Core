using System;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Hosting.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.Annotation;
using ApacheTech.VintageMods.Core.Services.Configuration.Contracts;
using ApacheTech.VintageMods.Core.Services.Configuration.Extensions;
using ApacheTech.VintageMods.Core.Services.Configuration.WritableJsonFile;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class HostExtensions
    {
        /// <summary>
        ///     Registers the game's API within the DI container.
        /// </summary>
        /// <param name="api">The game's core API.</param>
        /// <param name="registrar">The registrar to use, to register the API for the current app-side.</param>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public static IServiceCollection RegisterAPI(this IServiceCollection services, ICoreAPI api, IRegistrar registrar)
        {
            registrar.Register(api, services);
            return services;
        }

        public static void AddAnnotatedServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly
                .GetTypesWithAttribute<RegisteredServiceAttribute>()
                .ToList();

            foreach (var (type, attribute) in types)
            {
                var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceScope);
                services.Add(descriptor);
            }
        }

        public static void AddGlobalConfiguration(this IServiceCollection services, string fileName)
        {
            services.AddSingleton(sp => sp.GetConfigurationBuilder(fileName, FileScope.Global).BuildGlobalConfig());
        }

        /// <summary>
        ///     Registers a file with a per-world scope, within the FileSystem Service, and adds the file
        ///     as a <see cref="IWorldConfiguration"/> to the service collection, so that it can be accessed
        ///     via dependency injection.
        /// </summary>
        /// <param name="services">The service collection to add the file to.</param>
        /// <param name="fileName">The name of the file to add.</param>
        public static void AddWorldConfiguration(this IServiceCollection services, string fileName)
        {
            services.AddSingleton(sp => sp.GetConfigurationBuilder(fileName, FileScope.World).BuildWorldConfig());
        }

        private static IConfigurationBuilder GetConfigurationBuilder(this IServiceProvider sp, string fileName, FileScope scope)
        {
            var fileSystemService = sp.GetRequiredService<IFileSystemService>();
            var configFile = fileSystemService
                .RegisterFile(fileName, scope)
                .GetJsonFile(fileName)
                .AsFileInfo();

            return new ConfigurationBuilder()
                .SetBasePath(configFile.DirectoryName)
                .AddWritableJsonFile(configFile.Name, optional: false, reloadOnChange: true);
        }
    }
}