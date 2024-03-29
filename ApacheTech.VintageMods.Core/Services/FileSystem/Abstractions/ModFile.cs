﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ApacheTech.VintageMods.Core.Services.EmbeddedResources;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions
{
    /// <summary>
    ///     A wrapper of a <see cref="FileInfo" /> for a specific file on on the filesystem. This class cannot be inherited.
    /// </summary>
    public abstract class ModFile : ModFileBase, IModFile
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFile" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        protected ModFile(string filePath) : base(filePath)
        {
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModFile" /> class.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        protected ModFile(FileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel" />, populated with data from this file.</returns>
        public abstract TModel ParseAs<TModel>() where TModel : class, new();

        /// <summary>
        /// Deserialises the specified file as a strongly-typed object.
        /// The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel" />, populated with data from this file.</returns>
        public abstract Task<TModel> ParseAsAsync<TModel>() where TModel : class, new();

        /// <summary>
        ///     Deserialises the specified file as a collection of a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}" />, populated with data from this file.</returns>
        public abstract IEnumerable<TModel> ParseAsMany<TModel>() where TModel : class, new();

        /// <summary>
        /// Deserialises the specified file as a collection of a strongly-typed object.
        /// The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}" />, populated with data from this file.</returns>
        public abstract Task<IEnumerable<TModel>> ParseAsManyAsync<TModel>() where TModel : class, new();

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        public abstract void SaveFrom<TModel>(TModel instance) where TModel : class, new();

        /// <summary>
        /// Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        /// <returns>Task.</returns>
        public abstract Task SaveFromAsync<TModel>(TModel instance) where TModel : class, new();

        /// <summary>
        /// Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        /// <returns>Task.</returns>
        public abstract Task SaveFromAsync<TModel>(IEnumerable<TModel> collection) where TModel : class, new();

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        public abstract void SaveFrom<TModel>(IEnumerable<TModel> collection) where TModel : class, new();

        /// <summary>
        ///     Disembeds the file from a specific assembly.
        /// </summary>
        /// <param name="assembly">The assembly to disembed the file from.</param>
        public void DisembedFrom(Assembly assembly)
        {
            var service = ModServices.IOC.Resolve<IEmbeddedResourcesService>();
            service.DisembedResource(assembly, ModFileInfo.Name, ModFileInfo.FullName);
        }
    }
}