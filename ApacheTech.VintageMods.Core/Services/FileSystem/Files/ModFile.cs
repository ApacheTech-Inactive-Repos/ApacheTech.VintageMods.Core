using System.Collections.Generic;
using System.IO;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Files
{
    /// <summary>
    ///     A wrapper of a <see cref="FileInfo" /> for a specific file on on the filesystem. This class cannot be inherited.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class ModFile : IModFile
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        protected ModFile(string filePath)
        {
            ModFileInfo = new FileInfo(filePath);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFileInfo" /> class.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        protected ModFile(FileInfo fileInfo)
        {
            ModFileInfo = fileInfo;
        }

        /// <summary>
        ///     Gets the type of the file, be it JSON, or Binary.
        /// </summary>
        /// <value>The type of the file.</value>
        internal abstract FileType FileType { get; }

        protected FileInfo ModFileInfo { get; }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel" />, populated with data from this file.</returns>
        public abstract TModel ParseAs<TModel>() where TModel : class, new();

        /// <summary>
        ///     Deserialises the specified file as a collection of a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}" />, populated with data from this file.</returns>
        public abstract IEnumerable<TModel> ParseAsMany<TModel>() where TModel : class, new();

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        public abstract void SaveFrom<TModel>(TModel instance) where TModel : class, new();

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        public abstract void SaveFrom<TModel>(IEnumerable<TModel> collection) where TModel : class, new();
    }
}