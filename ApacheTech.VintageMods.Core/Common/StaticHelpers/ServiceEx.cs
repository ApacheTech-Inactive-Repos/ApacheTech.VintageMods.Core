using ApacheTech.VintageMods.Core.Abstractions.ModSystems.Composite;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Services.FileSystem;
using ApacheTech.VintageMods.Core.Services.HarmonyPatches;
using ApacheTech.VintageMods.Core.Services.MefLab;
using ApacheTech.VintageMods.Core.Services.Network;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    /// <summary>
    ///     Globally accessible services, populated through the IOC Container. If a derived
    ///     instance of type <see cref="VintageModLoader"/> has not been created within the
    ///     Application layer, these services will not be available, and will have to be
    ///     instantiated manually.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceEx
    {
        /// <summary>
        ///     Gets the mod information.
        /// </summary>
        /// <value>The mod information.</value>
        public static IVintageModInfo ModInfo { get; internal set; }
        
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
    }
}