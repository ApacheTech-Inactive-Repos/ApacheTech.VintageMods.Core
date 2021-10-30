using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching;
using ApacheTech.VintageMods.Core.Services.MefLab;
using ApacheTech.VintageMods.Core.Services.Network;
using JetBrains.Annotations;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting
{
    /// <summary>
    ///     Globally accessible services, populated through the IOC Container. If a derived
    ///     instance of type <see cref="VintageModLoader"/> has not been created within the
    ///     Application layer, these services will not be available, and will have to be
    ///     instantiated manually.<br/><br/>
    ///
    ///     Globally static objects like this are required because no <see cref="ModSystem"/>
    ///     can make use of dependency injection directly, because they are instantiated by
    ///     the game. Instead of using traditional singleton instances, where we'd need to
    ///     add boilerplate every time it's needed, we can cache the instances here, and then
    ///     used them anywhere they are needed, in lieu of being able to pass it in through
    ///     an importing constructor.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ModServices
    {
        /// <summary>
        ///     Provides a means for handling files, including embedded resources, used within a mod.
        /// </summary>
        /// <remarks>
        ///      It is up to each mod to configure, and register their own files.
        ///      This template doesn't enforce any file policy upon derived mods.
        /// </remarks>
        public static IFileSystemService FileSystem => IOC.Resolve<IFileSystemService>();

        /// <summary>
        ///     Provides methods of applying Harmony patches to the game.
        /// </summary>
        /// <remarks>
        ///     By default, all annotated [HarmonyPatch] classes in the executing assembly will
        ///     be processed at launch. Manual patches can be processed later on at runtime.
        /// </remarks>
        public static IHarmonyPatchingService Harmony => IOC.Resolve<IHarmonyPatchingService>();

        /// <summary>
        ///     Provides narrowed scope access to network channels within the game.
        /// </summary>
        /// <remarks>
        ///     Adds two channels, by default; one called "VintageMods", which can be
        ///     used as an IPC pipe between VintageMods mods, and one for with the ModId
        ///     of the calling Mod. Other channels can be registered, as needed.
        /// </remarks>
        public static INetworkService Network => IOC.Resolve<INetworkService>();

        /// <summary>
        ///     Provides methods for resolving dependencies, through the Managed Extensibility Framework (MEF).
        /// </summary>
        /// <remarks>
        ///     By default, the mod's own assembly is added to the Aggregate Catalogue.
        ///     This is a transient service, so if it is used through DI, only the
        ///     default assembly will be present. Further Assemblies, and Directories
        ///     can be added as needed. 
        /// </remarks>
        public static IMefLabService MefLab => IOC.Resolve<IMefLabService>();

        /// <summary>
        ///     Gets the IOC Resolver for the Server.
        /// </summary>
        /// <value>The IOC Resolver for the Server.</value>
        internal static IDependencyResolver ServerIOC { private get; set; }

        /// <summary>
        ///     Gets the IOC Resolver for the Client.
        /// </summary>
        /// <value>The IOC Resolver for the Client.</value>
        internal static IDependencyResolver ClientIOC { private get; set; }

        /// <summary>
        ///     Gets the IOC Resolver for the current app-side.
        /// </summary>
        /// <value>The IOC Resolver for the current app-side.</value>
        public static IDependencyResolver IOC => ApiEx.Side.IsClient() ? ClientIOC : ServerIOC;
    }
}