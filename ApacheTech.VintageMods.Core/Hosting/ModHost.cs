using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.DotNet;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// ReSharper disable VirtualMemberNeverOverridden.Global
// ReSharper disable UnusedParameter.Global

namespace ApacheTech.VintageMods.Core.Hosting
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
        private IServiceCollection _services;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModHost" /> class.
        /// </summary>
        protected ModHost()
        {
            ApiEx.ThreadSideCache.Clear();
            var assembly = AssemblyEx.ModAssembly = Assembly.GetAssembly(GetType());
            ApiEx.ModInfo = assembly.GetCustomAttribute<VintageModInfoAttribute>();

            StartPreUniversal();
        }

        #region Universal Configuration

        private void StartPreUniversal()
        {
            _services = new ServiceCollection();
            _services.Configure(ConfigureCoreModServices);
        }

        private static void ConfigureCoreModServices(IServiceCollection services)
        {
            services.RegisterSingleton(ApiEx.ModInfo);
            services.RegisterAnnotatedServicesFromAssembly(AssemblyEx.GetCoreAssembly());
        }

        #endregion

        #region Server Configuration

        /// <summary>
        ///     Gets a list of all client-side features within the mod that need registration.
        /// </summary>
        private List<IServerServiceRegistrar> ServerFeatures { get; set; }

        private void BuildServerHost(ICoreServerAPI sapi)
        {
            //  1. Set ApiEx endpoints.
            ApiEx.Server = sapi;
            ApiEx.ServerMain = (ServerMain)sapi.World;
            ModPaths.Initalise();

            ServerFeatures = (AssemblyEx.GetModAssembly()
                .GetTypes()
                .Where(p => typeof(IServerServiceRegistrar).IsAssignableFrom(p) && !p.IsAbstract)
                .Select(p => (IServerServiceRegistrar)Activator.CreateInstance(p))).ToList();
            
            //  2. Configure game API services.
            _services.Configure(ServerApiRegistrar.RegisterServerApiEndpoints);

            //  3. Register all ModSystems within the mod. Will also self-reference this ModHost. 
            _services.RegisterModSystems();

            //  4. Delegate mod service configuration to derived class.
            _services.Configure(ConfigureServerModServices);

            //  5. Register all features that need registering. 
            ServerFeatures.ForEach(p => p.ConfigureServerModServices(_services));

            //  6. Build IOC Container.
            ModServices.ServerIOC = _services.Build();

            // ONLY NOW ARE SERVICES AVAILABLE

            //  7. Delegate mod PreStart to derived class.
            StartPreServerSide(sapi);

            //  8. Delegate PreStart to features.
            ServerFeatures.ForEach(p => p.StartPreServerSide(sapi));
        }
        
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

        #region Client Configuration

        /// <summary>
        ///     Gets a list of all client-side features within the mod that need registration.
        /// </summary>
        private List<IClientServiceRegistrar> ClientFeatures { get; set; }
            

        private void BuildClientHost(ICoreClientAPI capi)
        {
            //  1. Set ApiEx endpoints.
            ApiEx.Client = capi;
            ApiEx.ClientMain = (ClientMain)capi.World;
            ModPaths.Initalise();

            ClientFeatures = AssemblyEx
                .GetModAssembly()
                .GetTypes()
                .Where(p => typeof(IClientServiceRegistrar).IsAssignableFrom(p) && !p.IsAbstract)
                .Select(p => (IClientServiceRegistrar)Activator.CreateInstance(p))
                .ToList();

            //  2. Configure game API services.
            _services.Configure(ClientApiRegistrar.RegisterClientApiEndpoints);

            //  3. Register all ModSystems within the mod. Will also self-reference this ModHost. 
            _services.RegisterModSystems();

            //  4. Delegate mod service configuration to derived class.
            _services.Configure(ConfigureClientModServices);

            //  5. Register all features that need registering. 
            ClientFeatures.ForEach(p => p.ConfigureClientModServices(_services));

            //  6. Build IOC Container.
            ModServices.ClientIOC = _services.Build();

            // ONLY NOW ARE SERVICES AVAILABLE

            //  7. Delegate mod PreStart to derived class.
            StartPreClientSide(capi);

            //  8. Delegate PreStart to features.
            ClientFeatures.ForEach(p => p.StartPreClientSide(capi));
        }

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
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Start(ICoreAPI api)
        {
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    ServerFeatures.ForEach(p => p.Start(api));
                    break;
                case EnumAppSide.Client:
                    ClientFeatures.ForEach(p => p.Start(api));
                    ((ICoreClientAPI)api).Event.LeftWorld += Dispose;
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
            DisposeOnLeaveWorld();
            base.Dispose();
        }

        private static void DisposeOnLeaveWorld()
        {
            ModSettings.Dispose();
            ModServices.IOC?.Dispose();
            ApiEx.Dispose();
            AssemblyEx.GetModAssembly().NullifyOrphanedStaticMembers();
            AssemblyEx.GetCoreAssembly().NullifyOrphanedStaticMembers();
            AssemblyEx.ModAssembly = null;
        }

        #endregion
    }
}