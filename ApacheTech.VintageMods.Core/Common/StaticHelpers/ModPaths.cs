using System;
using System.IO;
using System.Reflection;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ModPaths
    {
        private static Assembly ModAssembly { get; }

        /// <summary>
        /// 	Initialises static members of the <see cref="ModPaths" /> class.
        /// </summary>
        static ModPaths()
        {
            ModAssembly = AssemblyEx.GetModAssembly();
            ModDataRootPath = CreateDirectory(Path.Combine(VintageModsRootPath, ApiEx.ModInfo.RootDirectoryName));
            ModDataGlobalPath = CreateDirectory(Path.Combine(ModDataRootPath, "Global"));
            ModDataWorldPath = CreateDirectory(Path.Combine(ModDataRootPath, "World", ApiEx.Current.World.SavegameIdentifier));
            ModRootPath = Path.GetDirectoryName(ModAssembly.Location)!;
            ModAssetsPath = Path.Combine(Path.GetDirectoryName(ModAssembly.Location)!, "assets");
        }

        /// <summary>
        ///     Gets the root path for all VintageMods mod files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string VintageModsRootPath { get; } = CreateDirectory(Path.Combine(GamePaths.DataPath, "ModData", "VintageMods"));

        /// <summary>
        ///     Gets the path used for storing data files for a particular mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataRootPath { get; }

        /// <summary>
        ///     Gets the path used for storing global data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataGlobalPath { get; }

        /// <summary>
        ///     Gets the path used for storing per-world data files.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModDataWorldPath { get; internal set; }

        /// <summary>
        ///     Gets the path that the mod library is stored in.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModRootPath { get; }

        /// <summary>
        ///     Gets the main asset origin directory for the mod.
        /// </summary>
        /// <value>A path on the filesystem, used to store mod files.</value>
        public static string ModAssetsPath { get; }

        internal static string CreateDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists) return dir.FullName;
            ApiEx.Current?.Logger.VerboseDebug($"[VintageMods] Creating folder: {dir}");
            dir.Create();
            return dir.FullName;
        }

        internal static string GetScopedPath(FileScope scope, string fileName)
        {
            var directory = scope switch
            {
                FileScope.Global => ModDataGlobalPath,
                FileScope.World => ModDataWorldPath,
                FileScope.Local => ModRootPath,
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };

            if (scope is not FileScope.Local) return Path.Combine(directory, fileName);
            if (File.Exists(directory)) return Path.Combine(directory, fileName);

            var files = Directory.GetFiles(ModPaths.ModRootPath, fileName, SearchOption.AllDirectories);
            return files.Length switch
            {
                1 => files[0],
                < 1 => throw new FileNotFoundException(
                    // TODO: Move to lang file.
                    Lang.Get("Local file, `{0}`, does not exist within the mod folder.", fileName)),
                > 1 => throw new FileLoadException(
                    Lang.Get("Local file, `{0}`, is duplicated within the mod folder.", fileName))
            };
        }
    }
}