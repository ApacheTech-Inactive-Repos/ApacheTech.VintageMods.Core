using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ModPaths
    {
        /// <summary>
        ///     Gets the root path for all VintageMods mod files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string VintageModsRootPath { get; internal set; }

        /// <summary>
        ///     Gets the path used for storing data files for a particular mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataRootPath { get; internal set; }

        /// <summary>
        ///     Gets the path used for storing global data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataGlobalPath { get; internal set; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataWorldPath { get; internal set; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModAssetsPath { get; internal set; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModRootPath { get; internal set; }

    }
}