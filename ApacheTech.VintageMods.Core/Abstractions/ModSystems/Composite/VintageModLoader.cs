using System;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.Extensions;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.DependencyInjection;
using ApacheTech.VintageMods.Core.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.DependencyInjection.Registrars;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

// =========================================================================================================================

/***
 * QUESTION: Will I ever actually use a local file-based SQLite database for any of my mods?
 *
 * Theory Casting:
 *
 * BENEFITS:
 *  - Hugely scalable.
 *  - Able to store pretty much everything in one single place... even for multiple mods.
 *  - Able to easily separate data for different players, in different servers.
 *  - Could have major advantages for server-side mods, where information about multiple players is needed to be kept.
 *
 * DRAWBACKS:
 *  - Tricky to maintain, and upkeep.
 *  - There's zero chance of migrating local databases.
 *  - Only large-scale mods will ever need a database.
 *  - Nigh-on useless, client-side.
 *
 * CONSENSUS:
 *
 * While it may be useful to have a database for large scale server-side mods, that's the only place it would be worthwhile.
 * With a database needing so much infrastructure, it may be better to leave the implementation of a server-side database
 * until it is needed. I don't think there's much point in scaffolding Generic Repositories and Unit of Work abstractions,
 * when it's possible that no mod I ever create will need them.
 **/

// =========================================================================================================================

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems.Composite
{
    /// <summary>
    ///     Only one derived instance of this base-class should be added to any single mod within
    ///     the VintageMods domain. This base-class will enable Dependency Injection, and add all
    ///     of the domain services. Derived instances should only have minimal functionality, 
    ///     instantiating, and adding Application specific services to the IOC Container.
    /// </summary>
    /// <seealso cref="UniversalModSystem" />
    public abstract class VintageModLoader : UniversalModSystem
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="VintageModLoader" /> class.
        /// </summary>
        protected VintageModLoader()
        {
            var assembly = ApiEx.ModAssembly = Assembly.GetAssembly(GetType());
            ApiEx.ModInfo = assembly.GetCustomAttribute<VintageModInfoAttribute>();
        }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);
            switch (UApi.Side)
            {
                case EnumAppSide.Server:
                    ApiEx.ServerIOC =
                        new DependencyResolverBuilder()
                        .RegisterAPI(api, ServerRegistrar.CreateInstance())
                        .Configure(ConfigureRequiredServices)
                        .Configure(ConfigureModServices)
                        .Build();
                    StartPreServerSide(Sapi);
                    break;
                case EnumAppSide.Client:
                    ApiEx.ClientIOC =
                        new DependencyResolverBuilder()
                        .RegisterAPI(api, ClientRegistrar.CreateInstance())
                        .Configure(ConfigureRequiredServices)
                        .Configure(ConfigureModServices)
                        .Build();
                    StartPreClientSide(Capi);
                    break;
                case EnumAppSide.Universal:
                    // Does not exist. Game breaks SRP/ISP for Enums.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">The core API implemented by the client. The main interface for accessing the client. Contains all sub-components, and some miscellaneous methods.</param>
        protected virtual void StartPreClientSide(ICoreClientAPI capi)
        {
            // Left empty as an optional expansion method.
        }

        /// <summary>
        ///     Called on the server, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="sapi">The core API implemented by the server. The main interface for accessing the server. Contains all sub-components, and some miscellaneous methods.</param>
        protected virtual void StartPreServerSide(ICoreServerAPI sapi)
        {
            // Left empty as an optional expansion method.
        }

        /// <summary>
        ///     Services that are added here will be available on all mods.
        /// </summary>
        private void ConfigureRequiredServices(IServiceCollection services)
        {
            services.AddSingleton(ApiEx.ModInfo);

            var types = ApiEx.GetCoreAssembly()
                .GetTypesWithAttribute<RegisteredServiceAttribute>()
                .ToList();

            foreach (var (type, attribute) in types)
            {
                var descriptor = new ServiceDescriptor(attribute.ServiceType, type, attribute.ServiceScope);
                services.Add(descriptor);
            }
        }

        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        protected abstract void ConfigureModServices(IServiceCollection services);

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.
        /// 
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers
        ///     will be called in the same execution order.
        /// 
        ///     Default execute order of some survival mod parts.
        /// 
        ///     World Gen:
        ///      - GenTerra: 0
        ///      - RockStrata: 0.1
        ///      - Deposits: 0.2
        ///      - Caves: 0.3
        ///      - BlockLayers: 0.4
        /// 
        ///     Asset Loading:
        ///      - Json Overrides loader: 0.05
        ///      - Load hardcoded mantle block: 0.1
        ///      - Block and Item Loader: 0.2
        ///      - Recipes (Smithing, Knapping, ClayForming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        /// <returns>System.Double.</returns>
        public override double ExecuteOrder()
        {
            return -1;
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers.
        /// </summary>
        public override void Dispose()
        {
            (ApiEx.IOC as IDisposable)?.Dispose();
            ApiEx.Dispose();
            base.Dispose();
        }
    }
}