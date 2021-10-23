using System;
using ApacheTech.VintageMods.Core.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Dependency Injection Services.
    /// </summary>
    public static class IOC
    {
        /// <summary>
        ///     Gets the DI container for the mod.
        /// </summary>
        /// <value>The <see cref="IServiceCollection" /> DI container.</value>
        internal static IServiceCollection Container { get; } = new ServiceCollection();

        /// <summary>
        ///     Gets the service provider, used to resolve instances of services within the DI Container.
        /// </summary>
        /// <value>The <see cref="IServiceProvider" /> service. provider.</value>
        internal static IServiceProvider Provider { get; private set; }

        public static TService Resolve<TService>()
        {
            return Provider.GetRequiredService<TService>();
        }
        public static TService Resolve<TService>(params object[] args) where TService : class
        {
            return Provider.CreateInstance<TService>(args);
        }

        internal static IServiceCollection Configure(this IServiceCollection services,
            System.Action<IServiceCollection> callback)
        {
            callback(services);
            return services;
        }

        /// <summary>
        ///     Builds the specified DI container.
        /// </summary>
        /// <param name="services">The DI container to build.</param>
        internal static void Build(this IServiceCollection services)
        {
            Provider = services.BuildServiceProvider();
        }

        /// <summary>
        ///     Registers the game's API within the DI container.
        /// </summary>
        /// <param name="services">The DI Container.</param>
        /// <param name="api">The game's core API.</param>
        /// <returns>Returns the same instance of <see cref="IServiceCollection" /> that it was passed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Side - Fatal error found while registering the API within the DI Container.
        /// </exception>
        internal static IServiceCollection RegisterAPI(this IServiceCollection services, ICoreAPI api)
        {
            ApiEx.Universal = api;
            services.AddSingleton((ICoreAPICommon)api);
            services.AddSingleton(ApiEx.Universal);
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    ApiEx.Server = (ICoreServerAPI)api;
                    ApiEx.ServerMain = (ServerMain)ApiEx.Server.World;
                    services.AddSingleton(ApiEx.Server);
                    services.AddSingleton(ApiEx.Server.World);
                    services.AddSingleton(ApiEx.ServerMain);
                    break;
                case EnumAppSide.Client:
                    ApiEx.Client = (ICoreClientAPI)api;
                    ApiEx.ClientMain = (ClientMain)ApiEx.Client.World;
                    services.AddSingleton(ApiEx.Client);
                    services.AddSingleton(ApiEx.Client.World);
                    services.AddSingleton(ApiEx.ClientMain);
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(api.Side), api.Side,
                        "Fatal error found while registering the API within the DI Container.");
            }
            return services;
        }
    }
}