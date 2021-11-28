using System.Collections.Generic;
using System.IO;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.FileSystem.Registration;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Services.FileSystem
{
    /// <summary>
    ///     Provides a means for handling files, including embedded resources, used within a mod.
    /// </summary>
    [RegisteredService(ServiceLifetime.Singleton, typeof(IFileSystemService))]
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class FileSystemService : IFileSystemService
    {
        private readonly IModFileRegistrar _registrar;

        private readonly Dictionary<string, ModFileBase> _registeredFiles = new();

        public FileSystemService(IModFileRegistrar registrar)
        {
            _registrar = registrar;
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
            // TODO: Partially refactored... but just kicked the can down the road.
            //!? Open/Closed issues need to be resolved properly, rather than just pushed into a separate helper class.
            var file = new FileInfo(_registrar.GetScopedPath(fileName, scope));
            var fileType = _registrar.ParseFileType(file);
            var modFile = _registrar.InstantiateModFile(fileType, file);
            _registeredFiles.Add(fileName, modFile);
            if (file.Exists) return this;
            _registrar.CopyFileToOutputDirectory(fileName, file, fileType);
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
    }
}