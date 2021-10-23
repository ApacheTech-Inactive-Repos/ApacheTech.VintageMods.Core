using System.Collections.Generic;
using System.IO;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Vintagestory.API.Datastructures;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.FileAdaptors
{
    /// <summary>
    ///     Represents a JSON file, used by the mod. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="ModFile" />
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class JsonModFile : ModFile, IJsonModFile
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonModFile"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public JsonModFile(string filePath) : base(filePath)
        {
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonModFile"/> class.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        public JsonModFile(FileInfo fileInfo) : base(fileInfo)
        {
        }

        /// <summary>
        ///     Gets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public override FileType FileType => FileType.Json;

        /// <summary>
        ///     Deserialises the specified file as a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <typeparamref name="TModel" />, populated with data from this file.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override TModel ParseAs<TModel>()
        {
            return JsonConvert.DeserializeObject<TModel>(File.ReadAllText(ModFileInfo.FullName));
        }

        /// <summary>
        ///     Deserialises the specified file as a collection of a strongly-typed object.
        ///     The consuming type must have a paramaterless constructor.
        /// </summary>
        /// <typeparam name="TModel">The type of object to deserialise into.</typeparam>
        /// <returns>An instance of type <see cref="IEnumerable{TModel}" />, populated with data from this file.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerable<TModel> ParseAsMany<TModel>()
        {
            return JsonConvert.DeserializeObject<IEnumerable<TModel>>(File.ReadAllText(ModFileInfo.FullName));
        }

        /// <summary>
        ///     Serialises the specified instance, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="instance">The instance of the object to serialise.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SaveFrom<TModel>(TModel instance)
        {
            var json = JsonConvert.SerializeObject(instance, Formatting.Indented);
            File.WriteAllText(ModFileInfo.FullName, json);
        }

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SaveFrom<TModel>(IEnumerable<TModel> collection)
        {
            SaveFrom(collection, Formatting.Indented);
        }

        /// <summary>
        ///     Opens the file, reads all lines of text, and then closes the file.
        /// </summary>
        /// <returns>A <see cref="string" />, containing all lines of text within the file.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string ReadAllText()
        {
            return File.ReadAllText(ModFileInfo.FullName);
        }

        /// <summary>
        ///     Serialises the specified collection of objects, and saves the resulting data to file.
        /// </summary>
        /// <typeparam name="TModel">The type of the object to serialise.</typeparam>
        /// <param name="collection">The collection of the objects to save to a single file.</param>
        /// <param name="formatting">The JSON formatting style to use when serialising the data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveFrom<TModel>(IEnumerable<TModel> collection, Formatting formatting)
        {
            var json = JsonConvert.SerializeObject(collection, formatting);
            File.WriteAllText(ModFileInfo.FullName, json);
        }

        /// <summary>
        ///     Parses the file into Vintage Story's bespoke JsonObject wrapper.
        /// </summary>
        /// <returns>An instance of type <see cref="JsonObject" />, populated with data from this file.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public JsonObject ParseAsJsonObject()
        { 
            return JsonObject.FromJson(File.ReadAllText(ModFileInfo.FullName));
        }
    }
}