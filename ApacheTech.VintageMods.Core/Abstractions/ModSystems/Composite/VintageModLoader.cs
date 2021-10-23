using System;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem;
using ApacheTech.VintageMods.Core.Services.FileSystem.Registration;
using ApacheTech.VintageMods.Core.Services.HarmonyPatches;
using ApacheTech.VintageMods.Core.Services.MefLab;
using ApacheTech.VintageMods.Core.Services.Network;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

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
            var assembly = AssemblyEx.ModAssembly = Assembly.GetAssembly(GetType());
            ServiceEx.ModInfo = assembly.GetCustomAttribute<VintageModInfoAttribute>();
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

            IOC.Container
                .RegisterAPI(api)
                .Configure(ConfigureRequiredServices)
                .Configure(ConfigureModServices)
                .Build();

            ApplyHarmonyPatches(AssemblyEx.GetModAssembly());
        }

        /// <summary>
        ///     Services that are added here will be available on all mods.
        /// </summary>
        private void ConfigureRequiredServices(IServiceCollection services)
        {
            services.AddSingleton(ServiceEx.ModInfo);

            services.AddSingleton<IEmbeddedResourcesService, EmbeddedResourcesService>();
            services.AddSingleton<IModFileRegistrar, ModFileRegistrar>();
            services.AddSingleton<IFileSystemService, FileSystemService>();

            services.AddSingleton<IHarmonyPatchingService, HarmonyPatchingService>();
            services.AddSingleton<INetworkService, NetworkService>();

            services.AddTransient<IMefLabService, MefLabService>();
        }

        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        protected abstract void ConfigureModServices(IServiceCollection services);

        /// <summary>
        ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
        ///     be processed at launch. Manual patches can be processed later on at runtime.
        /// </summary>
        private void ApplyHarmonyPatches(Assembly assembly)
        {
            try
            {
                var harmony = ServiceEx.Harmony.CreateOrUseInstance(assembly.FullName);
                harmony.PatchAll(assembly);
                Api.Logger.Notification($"\t{assembly.GetName()} - Patched Methods:");
                foreach (var method in harmony.GetPatchedMethods())
                    Api.Logger.Notification($"\t\t{method.FullDescription()}");
            }
            catch (Exception ex)
            {
                Api.Logger.Error($"[VintageMods] {ex}");
            }
        }

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
            ServiceEx.Harmony.UnpatchAll();
            base.Dispose();
        }
    }
}
