﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Vintagestory.API.Datastructures;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a JSON file on the filesystem.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IJsonModFile : IModFile, ITextModFile
    {
        /// <summary>
        ///     Parses the file into Vintage Story's bespoke JsonObject wrapper.
        /// </summary>
        /// <returns>An instance of type <see cref="JsonObject"/>, populated with data from this file.</returns>
        public JsonObject ParseAsJsonObject();

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        /// <param name="formatting">The JSON formatting style to use when serialising the data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveFrom<TModel>(IEnumerable<TModel> collection, Formatting formatting);
    }
}