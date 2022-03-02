using System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using SmartAssembly.Attributes;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Hosting.Configuration.Extensions
{
    [DoNotPruneType]
    public static class FileSystemServiceExtensions
    {
        /// <summary>
        ///     Registers a settings file with the FileSystem Service. This will copy a default implementation of the file from:
        ///      • An embedded resource.
        ///      • The mod's unpack directory.
        ///      • The mod's assets folder.
        /// 
        ///     If no default implementation can be found, a new file is created, at the correct location.
        /// </summary>
        /// <param name="fileSystem">The <see cref="IFileSystemService"/> to use to register the file.</param>
        /// <param name="fileName">The name of the file, including file extension.</param>
        /// <param name="scope">The scope of the file, be it global, or per-world.</param>
        public static IFileSystemService RegisterSettingsFile(
            this IFileSystemService fileSystem, 
            string fileName,
            FileScope scope)
        {
            fileSystem.RegisterFile(fileName, scope);
            var file = JsonSettingsFile.FromJsonFile(fileSystem.GetJsonFile(fileName));
            switch (scope)
            {
                case FileScope.Global:
                    ApiEx.Run(() => ModSettings.ClientGlobal = file, () => ModSettings.ServerGlobal = file);
                    break;
                case FileScope.World:
                    ApiEx.Run(() => ModSettings.ClientWorld = file, () => ModSettings.ServerWorld = file);
                    break;
                case FileScope.Local:
                    ApiEx.Run(() => ModSettings.ClientLocal = file, () => ModSettings.ServerLocal = file);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }

            return fileSystem;
        }
    }
}
