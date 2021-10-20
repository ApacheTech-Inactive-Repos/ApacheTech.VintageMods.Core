using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.FileSystem.Files;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    internal sealed class FileSystemService : IFileSystemService
    {
        private static ICoreAPI _api;

        private readonly IEmbeddedResourcesService _embeddedResources;

        private Dictionary<string, IModFile> RegisteredFiles { get; } = new();

        /// <summary>
        /// 	Initialises a new instance of the <see cref="FileSystemService" /> class.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="embeddedResources">The embedded resources.</param>
        public FileSystemService(ICoreAPI api, IEmbeddedResourcesService embeddedResources)
        {
            _api = api;
            _embeddedResources = embeddedResources;
            var assembly = Assembly.GetExecutingAssembly();
            var attribute = assembly.GetCustomAttribute<VintageModInfoAttribute>();

            // Violates Open/Closed.
            var pathDict = new List<(string Global, string Path)>
            {
                (ModPaths.VintageModsRootPath, CreateDirectory(Path.Combine(GamePaths.DataPath, "ModData", "VintageMods"))),
                (ModPaths.ModDataRootPath, CreateDirectory(Path.Combine(ModPaths.VintageModsRootPath, attribute.RootDirectoryName))),
                (ModPaths.ModDataGlobalPath, CreateDirectory(Path.Combine(ModPaths.ModDataRootPath, "Global"))),
                (ModPaths.ModDataWorldPath, CreateDirectory(Path.Combine(ModPaths.ModDataRootPath, "World", api.World.SavegameIdentifier))),
                (ModPaths.ModRootPath, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)),
                (ModPaths.ModAssetsPath, Path.Combine(ModPaths.ModRootPath!, "assets"))
            };
            pathDict.ForEach(p => p.Global = p.Path);
        }

        /// <summary>
        ///     Registers a file with the FileSystem Service. This will copy a default implementation of the file from:
        ///      • An embedded resource.
        ///      • The mod's unpack directory.
        ///      • The mod's assets folder.
        ///
        ///     If no default implementation can be found, a new file is created, at the correct location.
        /// </summary>
        /// <param name="fileName">The name of the file, including file extension.</param>
        /// <param name="scope">The scope of the file, be it global, or per-world.</param>
        public void RegisterFile(string fileName, FileScope scope)
        {
            // Violates Open/Closed.
            var directory = scope switch
            {
                FileScope.Global => ModPaths.ModDataGlobalPath,
                FileScope.World => ModPaths.ModDataWorldPath,
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };

            var file = new FileInfo(Path.Combine(directory, fileName));

            // Violates Open/Closed.
            var fileType = file.Extension switch
            {
                "json" => FileType.Json,
                "data" => FileType.Json,
                "dat" => FileType.Binary,
                "bin" => FileType.Binary,
                "dll" => FileType.Binary,
                _ => throw new ArgumentOutOfRangeException()
            };

            // Violates Open/Closed.
            IModFile modFile = fileType switch
            {
                FileType.Json => new JsonModFile(file),
                FileType.Binary => new BinaryModFile(file),
                _ => throw new ArgumentOutOfRangeException()
            };
            RegisteredFiles.Add(fileName, modFile);

            // Violates Open/Closed.
            var assembly = Assembly.GetExecutingAssembly();
            if (_embeddedResources.ResourceExists(assembly, fileName))
            {
                _embeddedResources.DisembedResource(assembly, fileName, file.FullName);
                return;
            }

            // Violates Open/Closed.
            var locations = new List<string>
            {
                Path.Combine(ModPaths.ModAssetsPath, fileName),
                Path.Combine(ModPaths.ModRootPath, fileName)
            };

            // Violates Open/Closed.
            foreach (var location in locations.Where(File.Exists))
            {
                File.Copy(location, file.FullName);
                return;
            }

            // Violates Open/Closed.
            file.Create();
            if (fileType == FileType.Json)
            {
                using var writer = file.AppendText();
                writer.WriteLine("{\n\n}");
            }
        }

        private static string CreateDirectory(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                _api?.Logger.VerboseDebug($"[VintageMods] Creating folder: {dir}");
                dir.Create();
            }
            return dir.FullName;
        }

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IModFile"/> representation of the file, on disk.</returns>
        public IModFile GetRegisteredFile(string fileName)
        {
            if (RegisteredFiles.ContainsKey(fileName))
            {
                return RegisteredFiles[fileName];
            }

            throw new FileNotFoundException(
                Lang.Get("vmods:exceptions.file-not-registered-or-missing", fileName));
        }
    }
}