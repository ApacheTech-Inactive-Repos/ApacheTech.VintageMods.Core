﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.FileSystem.Extensions;
using SmartAssembly.Attributes;
using Vintagestory.API.Config;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    [RegisteredService(ServiceLifetime.Singleton, typeof(IFileSystemService))]
    [DoNotPruneType]
    internal sealed class FileSystemService : IFileSystemService
    {
        private readonly IEmbeddedResourcesService _embeddedResources;

        private readonly IDictionary<string, ModFileBase> _registeredFiles;

        public FileSystemService(IEmbeddedResourcesService embeddedResources)
        {
            _embeddedResources = embeddedResources;
            _registeredFiles = new Dictionary<string, ModFileBase>();
        }

        /// <summary>
        ///     Registers a file with the FileSystem Service. This will copy a default implementation of the file from:
        ///     • An embedded resource.
        ///     • The mod's unpack directory.
        ///     • The mod's assets folder.
        ///     If no default implementation can be found, a new file is created, at the correct location.
        /// </summary>
        /// <param name="fileName">The name of the file, including file extension.</param>
        /// <param name="scope">The scope of the file, be it global, or per-world.</param>
        public IFileSystemService RegisterFile(string fileName, FileScope scope)
        {
            var file = new FileInfo(ModPaths.GetScopedPath(scope, fileName));
            _registeredFiles.Add(fileName, file.CreateModFileWrapper());
            if (!file.Exists)
            {
                CopyFileToOutputDirectory(file);
            }
            return this;
        }

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IModFile" /> representation of the file, on disk.</returns>
        public object GetRegisteredFile(string fileName)
        {
            if (_registeredFiles.ContainsKey(fileName)) return _registeredFiles[fileName];
            throw new FileNotFoundException(
                Lang.Get("vmods:exceptions.file-not-registered-or-missing", fileName));
        }

        /// <summary>
        ///     Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <typeparam name="TFileType">The type of the file type to return as.</typeparam>
        /// <param name="fileName">The name of the file, including file extension.</param>
        /// <returns>Return an <see cref="TFileType" /> representation of the file, on disk.</returns>
        public TFileType GetRegisteredFile<TFileType>(string fileName) where TFileType : IModFileBase
        {
            return (TFileType)GetRegisteredFile(fileName);
        }

        /// <summary>
        /// Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IJsonModFile" /> representation of the file, on disk.</returns>
        public IJsonModFile GetJsonFile(string fileName)
        {
            return GetRegisteredFile<IJsonModFile>(fileName);
        }

        /// <summary>
        /// Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="IBinaryModFile" /> representation of the file, on disk.</returns>
        public IBinaryModFile GetBinaryFile(string fileName)
        {
            return GetRegisteredFile<IBinaryModFile>(fileName);
        }

        /// <summary>
        /// Retrieves a file that has previously been registered with the FileSystem Service.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Return an <see cref="ITextModFile" /> representation of the file, on disk.</returns>
        public ITextModFile GetTextFile(string fileName)
        {
            return GetRegisteredFile<ITextModFile>(fileName);
        }

        private void CopyFileToOutputDirectory(FileSystemInfo file)
        {
            var assembly = AssemblyEx.GetModAssembly();
            if (_embeddedResources.ResourceExists(assembly, file.Name))
            {
                _embeddedResources.DisembedResource(assembly, file.Name, file.FullName);
                return;
            }

            var locations = new List<string>
            {
                Path.Combine(ModPaths.ModAssetsPath, file.Name),
                Path.Combine(ModPaths.ModRootPath, file.Name)
            };
            foreach (var location in locations.Where(File.Exists))
            {
                if (file.Exists) return;
                File.Copy(location, file.FullName, false);
                return;
            }

            Task.Factory.StartNew(async () =>
            {
                using var writer = File.CreateText(file.FullName);
                if (file.ParseFileType() != FileType.Json) return;
                await writer.WriteLineAsync("[]");
            });
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
           ModSettings.Dispose();
            _registeredFiles.Clear();
        }
    }
}