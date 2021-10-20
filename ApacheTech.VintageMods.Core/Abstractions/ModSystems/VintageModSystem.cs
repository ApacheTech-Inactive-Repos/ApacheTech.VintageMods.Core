using System;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Common.Singletons;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.HarmonyPatches;
using ApacheTech.VintageMods.Core.Services.MefLab;
using ApacheTech.VintageMods.Core.Services.ModConfiguration;
using HarmonyLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems
{
    public abstract class VintageModSystem : UniversalModSystem
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="VintageModSystem" /> class.
        /// </summary>
        protected VintageModSystem()
        {
            var assembly = Assembly.GetExecutingAssembly();
            VintageModInfo = assembly.GetCustomAttribute<VintageModInfoAttribute>();
        }

        /// <summary>
        ///     Provides a means for handling files, including embedded resources, used within a mod.
        /// </summary>
        protected IFileSystemService FileSystem { get; private set; }

        /// <summary>
        ///     Information pertaining to this mod, as set in the <see cref="VintageModInfoAttribute" /> assembly level
        ///     attribution.
        /// </summary>
        protected IVintageModInfo VintageModInfo { get; }

        /// <summary>
        ///     A service that provides methods of applying Harmony patches to the game.
        /// </summary>
        protected IHarmonyPatchingService HarmonyPatches { get; private set; }

        /// <summary>
        ///     A services that provides methods for resolving dependencies, through MEF.
        /// </summary>
        protected IMefLabService MefLab { get; private set; }

        /// <summary>
        ///     Gets the mod configuration.
        /// </summary>
        protected IConfiguration ModConfiguration { get; private set; }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client. Cast to ICoreServerAPI or
        ///     ICoreClientAPI to access side specific features.
        /// </param>
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            // 1. Configure and build the Dependency Injection Container.
            IOC.Container
                .RegisterAPI(api)
                .Configure(ConfigureRequiredServices)
                .Configure(ConfigureModServices)
                .Build();

            // 2. Apply the Harmony Patches for the mod.
            //
            // By default, all annotated [HarmonyPatch] classes in the executing assembly will
            // be processed at launch. Manual patches can be processed later on at runtime.
            HarmonyPatches = IOC.Provider.GetRequiredService<IHarmonyPatchingService>();
            ApplyHarmonyPatches(Assembly.GetExecutingAssembly());

            // 3. Give the mod access to the the FileSystem Service.
            FileSystem = IOC.Provider.GetRequiredService<IFileSystemService>();

            // 4. Register global mod settings file.
            // 
            // Two copies of this file will exist. The default file can be used in future,
            // to do version controlling, for updates, as we can compare both files, and
            // see which version is currently installed, compared to which is being loaded.
            FileSystem.RegisterFile("appsettings.json", FileScope.Global);
            ModConfiguration = new ConfigurationBuilder()
                .SetBasePath(ModPaths.ModDataGlobalPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables("VMODS_")
                .AddEnvironmentVariables($"{VintageModInfo.ModId.ToUpperInvariant()}_")
                .Build();
            IOC.Container.AddSingleton(ModConfiguration);

            // 5. Give the mod access to the the MefLab Service.
            //
            // Enables MEF out of the gate, in any class, anywhere. By default, an
            // application level catalogue is added to the aggregate, as well as the
            // executing assembly. Further catalogues can be added, as needed.
            MefLab = IOC.Provider.GetRequiredService<IMefLabService>();
        }

        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private void ConfigureRequiredServices(IServiceCollection services)
        {
            services.AddSingleton<IEmbeddedResourcesService, EmbeddedResourcesService>();
            services.AddSingleton<IFileSystemService, FileSystemService>();
            services.AddSingleton<IMefLabService, MefLabService>();
            services.AddSingleton<IModConfigurationService, ModConfigurationService>();
            services.AddSingleton<IHarmonyPatchingService, HarmonyPatchingService>();
        }

        /// <summary>
        ///     Allows a mod to include Singleton, Scoped, or Transient services to the DI Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        protected virtual void ConfigureModServices(IServiceCollection services)
        {
            // Extracted, and left blank. This will allow mods to implement their own services,
            // while not worrying about whether or not they have called the base method.
        }

        /// <summary>
        ///     Applies the harmony patches for this mod.
        /// </summary>
        /// <param name="assembly">The assembly to scan for patches within.</param>
        protected virtual void ApplyHarmonyPatches(Assembly assembly)
        {
            try
            {
                var harmony = HarmonyPatches.CreateOrUseInstance(assembly.FullName);
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
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers.
        /// </summary>
        public override void Dispose()
        {
            HarmonyPatches.UnpatchAll();
            base.Dispose();
        }
    }
}

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

/***
 * QUESTION: Should I enforce appsettings.json on each mod?
 * QUESTION: Even if not, should I have a custom ModConfig Service that sets up Microsoft.Extensions.Configuration?
 *
 * Theory Casting:
 *
 * BENEFITS:
 *  - Settings files are there, ready and waiting, when needed.
 *  - Automatic reloading on change... means no FileSystem watcher needed.
 *  - Makes a mod more scalable.
 *  - Allows settings to be easily exposed to the user.
 *  - Can offer rudimentary version control.
 *
 * DRAWBACKS:
 *  - It's yet more to pack into each mod.
 *  - More files to maintain and clean up, on the user's PC.
 *  - Not every mod will need exposed, user-configurable settings.
 *  - It adds more complexity to every mod I make.
 *  - More to remember, when scaffolding a mod.
 *  - More to remember, when building a mod archive.
 *
 * CONSENSUS:
 *
 *  Probably worth it, as things like version control can be handled within the base class.
 *  Although, it will mean more upkeep for each mod, even if no exposed settings are needed.
 *  A compromise could be held in making appsettings.json, an optional include, within the
 *  build configuration. However, this does somewhat defeat the object of having a consistent
 *  layout for mods.
 **/

// =========================================================================================================================