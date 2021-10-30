using System;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting;
using ApacheTech.VintageMods.Core.Hosting.Registrars;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems.Composite
{
    /// <summary>
    ///     Only one derived instance of this base-class should be added to any single mod within
    ///     the VintageMods domain. This base-class will enable Dependency Injection, and add all
    ///     of the domain services. Derived instances should only have minimal functionality, 
    ///     instantiating, and adding Application specific services to the IOC Container.
    /// </summary>
    /// <seealso cref="UniversalModSystem" />
    public abstract class ModHost : UniversalModSystem
    {
        private IHostBuilder _hostBuilder;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModHost" /> class.
        /// </summary>
        protected ModHost()
        {
            var assembly = ApiEx.ModAssembly = Assembly.GetAssembly(GetType());
            ApiEx.ModInfo = assembly.GetCustomAttribute<VintageModInfoAttribute>();
            StartPreUniversal();
        }

        #region Universal Configuration

        private void StartPreUniversal()
        {
            _hostBuilder = new HostBuilder()
                .ConfigureServices(ConfigureCoreModServices);
        }

        private void ConfigureCoreModServices(IServiceCollection services)
        {
            services.AddSingleton(ApiEx.ModInfo);
            services.AddAnnotatedServicesFromAssembly(ApiEx.GetCoreAssembly());
        }

        #endregion

        #region Client Configuration

        private void BuildClientHost(ICoreClientAPI capi)
        {
            //  1. Set ApiEx endpoints.
            ApiEx.Client = capi;
            ApiEx.ClientMain = (ClientMain)capi.World;

            //  2. Delegate mod configuration building to derived class.
            _hostBuilder.ConfigureAppConfiguration(ConfigureClientModConfiguration);

            //  3. Configure game API services.
            _hostBuilder.ConfigureServices(p =>
                p.RegisterAPI(capi, ClientRegistrar.CreateInstance()));

            //  4. Delegate mod service configuration to derived class.
            _hostBuilder.ConfigureServices(ConfigureClientModServices);

            //  5. Build IOC Container.
            var host = _hostBuilder.Build();
            ModServices.ClientIOC = new DependencyResolver(host.Services);

            // ONLY NOW ARE SERVICES AVAILABLE

            // 6. Delegate mod PreStart to derived class.
            StartPreClientSide(capi);
        }

        /// <summary>
        ///     Configures providers for the configuration settings, on the client side.
        /// </summary>
        /// <param name="configurationBuilder">The as-of-yet un-built configuration builder.</param>
        protected virtual void ConfigureClientModConfiguration(IConfigurationBuilder configurationBuilder) { }

        /// <summary>
        ///     Configures any services that need to be added to the IO Container, on the client side.
        /// </summary>
        /// <param name="services">The as-of-yet un-built services container.</param>
        protected virtual void ConfigureClientModServices(IServiceCollection services) { }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        protected virtual void StartPreClientSide(ICoreClientAPI capi) { }

        #endregion

        #region Server Configuration

        private void BuildServerHost(ICoreServerAPI sapi)
        {
            //  1. Set ApiEx endpoints.
            ApiEx.Server = sapi;
            ApiEx.ServerMain = (ServerMain)sapi.World;

            //  2. Delegate mod configuration building to derived class.
            _hostBuilder.ConfigureAppConfiguration(ConfigureServerModConfiguration);

            //  3. Configure game API services.
            _hostBuilder.ConfigureServices(p =>
                p.RegisterAPI(sapi, ServerRegistrar.CreateInstance()));

            //  4. Delegate mod service configuration to derived class.
            _hostBuilder.ConfigureServices(ConfigureServerModServices);

            //  5. Build IOC Container.
            var host = _hostBuilder.Build();
            ModServices.ServerIOC = new DependencyResolver(host.Services);

            // ONLY NOW ARE SERVICES AVAILABLE

            //  6. Delegate mod PreStart to derived class.
            StartPreServerSide(sapi);
        }

        /// <summary>
        ///     Configures providers for the configuration settings, on the server side.
        /// </summary>
        /// <param name="configurationBuilder">The as-of-yet un-built configuration builder.</param>
        protected virtual void ConfigureServerModConfiguration(IConfigurationBuilder configurationBuilder) { }

        /// <summary>
        ///     Configures any services that need to be added to the IO Container, on the server side.
        /// </summary>
        /// <param name="services">The as-of-yet un-built services container.</param>
        protected virtual void ConfigureServerModServices(IServiceCollection services) { }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        protected virtual void StartPreServerSide(ICoreServerAPI sapi) { }

        #endregion

        #region Boilerplate

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void StartPre(ICoreAPI api)
        {
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    BuildServerHost(api as ICoreServerAPI);
                    break;
                case EnumAppSide.Client:
                    BuildClientHost(api as ICoreClientAPI);
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers
        ///     will be called in the same execution order. Default execute order of some survival mod parts.
        /// 
        ///     World Gen: 
        ///     - GenTerra: 0
        ///     - RockStrata: 0.1
        ///     - Deposits: 0.2
        ///     - Caves: 0.3
        ///     - BlockLayers: 0.4
        /// 
        ///     Asset Loading:
        ///     - Json Overrides loader: 0.05
        ///     - Load hardcoded mantle block: 0.1
        ///     - Block and Item Loader: 0.2
        ///     - Recipes (Smithing, Knapping, ClayForming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        public override double ExecuteOrder()
        {
            return -1;
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers
        /// </summary>
        public override void Dispose()
        {
            (ModServices.IOC as IDisposable)?.Dispose();
            ApiEx.Dispose();
            base.Dispose();
        }

        #endregion
    }
}