﻿using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a file on the filesystem.
    /// </summary>
    public interface IModFile : IModFileBase
    {
        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel"/>, populated with data from this file.</returns>
        public TModel ParseAs<TModel>() 
            where TModel : class, new();

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel"/>, populated with data from this file.</returns>
        public Task<TModel> ParseAsAsync<TModel>()
            where TModel : class, new();

        /// <summary>
        ///     Deserialises the specified file as a collection of a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}"/>, populated with data from this file.</returns>
        public IEnumerable<TModel> ParseAsMany<TModel>() 
            where TModel : class, new();

        /// <summary>
        ///     Deserialises the specified file as a collection of a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}"/>, populated with data from this file.</returns>
        public Task<IEnumerable<TModel>> ParseAsManyAsync<TModel>()
            where TModel : class, new();

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        public void SaveFrom<TModel>(TModel instance) 
            where TModel : class, new();

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        public Task SaveFromAsync<TModel>(TModel instance)
            where TModel : class, new();

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        public Task SaveFromAsync<TModel>(IEnumerable<TModel> collection) 
            where TModel : class, new();

        /// <summary>
        ///     Disembeds the file from a specific assembly.
        /// </summary>
        /// <param name="assembly">The assembly to disembed the file from.</param>
        public void DisembedFrom(Assembly assembly);
    }
}