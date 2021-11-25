using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApacheTech.Common.Extensions.Abstractions;
using ApacheTech.Common.Extensions.Annotation;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.FileSystem.FileAdaptors;
using JetBrains.Annotations;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Registration
{
    [RegisteredService(ServiceLifetime.Singleton, typeof(IModFileRegistrar))]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class ModFileRegistrar : IModFileRegistrar
    {
        private readonly IEmbeddedResourcesService _embeddedResources;

        public ModFileRegistrar(IEmbeddedResourcesService embeddedResources)
        {
            _embeddedResources = embeddedResources;
        }

        public void CopyFileToOutputDirectory(string fileName, FileInfo file, FileType fileType)
        {
            var assembly = AssemblyEx.GetModAssembly();
            if (_embeddedResources.ResourceExists(assembly, fileName))
            {
                _embeddedResources.DisembedResource(assembly, fileName, file.FullName);
                return;
            }
            
            var locations = new List<string>
            {
                Path.Combine(ModPaths.ModAssetsPath, file.Name),
                Path.Combine(ModPaths.ModRootPath, fileName)
            };
            foreach (var location in locations.Where(File.Exists))
            {
                if (file.Exists) return;
                File.Copy(location, file.FullName, false);
                return;
            }
            
            file.Create();
            if (fileType != FileType.Json) return;
            using var writer = file.AppendText();
            writer.WriteLine("{\n\n}");
        }

        public string GetScopedPath(string fileName, FileScope scope)
        {
            var path = scope switch
            {
                FileScope.Global => ModPaths.ModDataGlobalPath,
                FileScope.World => ModPaths.ModDataWorldPath,
                FileScope.Local => ModPaths.ModRootPath,
                _ => throw new ArgumentOutOfRangeException(nameof(scope), scope, null)
            };

            if (scope is not FileScope.Local) return Path.Combine(path, fileName);
            if (File.Exists(path)) return Path.Combine(path, fileName);

            var files = Directory.GetFiles(ModPaths.ModRootPath, fileName, SearchOption.AllDirectories);
            return files.Length switch
            {
                1 => files[0],
                < 1 => throw new FileNotFoundException(
                    Lang.Get("Local file, `{0}`, does not exist within the mod folder.", fileName)),
                > 1 => throw new FileLoadException(
                    Lang.Get("Local file, `{0}`, is duplicated within the mod folder.", fileName))
            };
        }

        public FileType ParseFileType(FileSystemInfo file)
        {
            return file.Extension switch
            {
                ".json" => FileType.Json,
                ".data" => FileType.Json,
                ".dat" => FileType.Binary,
                ".bin" => FileType.Binary,
                ".dll" => FileType.Binary,
                ".txt" => FileType.Text,
                ".md" => FileType.Text,
                _ => FileType.Binary
            };
        }

        public ModFileBase InstantiateModFile(FileType fileType, FileInfo file)
        {
            return fileType switch
            {
                FileType.Json => new JsonModFile(file),
                FileType.Binary => new BinaryModFile(file),
                FileType.Text => new TextModFile(file),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
