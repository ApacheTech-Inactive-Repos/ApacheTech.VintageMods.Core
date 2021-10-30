using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;
using Newtonsoft.Json;

namespace ApacheTech.VintageMods.Core.Services.Configuration.WritableJsonFile
{
    /// <summary>
    ///     A JSON file based <see cref="FileConfigurationProvider"/>.
    /// </summary>
    /// <seealso cref="NewtonsoftJsonConfigurationProvider" />
    public class WritableJsonConfigurationProvider : NewtonsoftJsonConfigurationProvider
    {
        /// <summary>
        ///     Initialises a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        public WritableJsonConfigurationProvider(NewtonsoftJsonConfigurationSource source) : base(source)
        {
        }

        /// <summary>
        ///     Sets a value for a given key.
        /// </summary>
        /// <param name="key">The configuration key to set.</param>
        /// <param name="value">The value to set.</param>
        public override void Set(string key, string value)
        {
            base.Set(key, value);
            var fileFullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            var json = File.ReadAllText(fileFullPath);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            SetValueRecursively(key, jsonObj, value);
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);    
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            var remainingSections = sectionPathKey.Split(new[] { ':' }, 2);
            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                jsonObj[currentSection] = value;
            }
        }
    }
}
