using System;
using System.ComponentModel;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Configuration.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.Configuration.ObservableFeatures;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartAssembly.Attributes;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Hosting.Configuration
{
    /// <summary>
    ///     Represents a settings file for the mod, in JSON format.
    /// </summary>
    /// <seealso cref="IJsonSettingsFile" />
    [DoNotPruneType]
    public class JsonSettingsFile : IJsonSettingsFile
    {
        private readonly IJsonModFile _file;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonSettingsFile"/> class.
        /// </summary>
        /// <param name="file">The underlying file, registered within the file system service.</param>
        public JsonSettingsFile(IJsonModFile file)
        {
            _file = file;
        }

        /// <summary>
        ///     Binds the specified feature to a POCO class object; dynamically adding an implementation of <see cref="INotifyPropertyChanged"/>, 
        ///     which saves changes to the underlying JSON file, whenever a property within the POCO is set.
        /// </summary>
        /// <remarks>
        ///     NOTE: Over-enthusiastic use of property setting within the POCO class, may result in excessive writes to the JSON file.
        /// </remarks>
        /// <typeparam name="T">The <see cref="Type"/> of object to parse the settings for the feature into.</typeparam>
        /// <param name="featureName">The name of the feature.</param>
        /// <returns>An object, that represents the settings for a given mod feature.</returns>
        public T Feature<T>(string featureName) where T: class, new()
        {
            try
            {
                var json = _file.ParseAs<JObject>();
                T featureObj;
                var obj = json.SelectToken($"$.Features.{featureName}");
                if (obj is null)
                {
                    featureObj = new T();
                    var args = new FeatureSettingsChangedEventArgs<T>(featureName, featureObj);
                    OnPropertyChanged(args);
                }
                else
                {
                    featureObj = obj.ToObject<T>();
                }

                var observer = ObservableFeature<T>.Bind(featureName, featureObj);
                observer.PropertyChanged += OnPropertyChanged;
                return featureObj;
            }
            catch (Exception exception)
            {
                ApiEx.Current.Logger.Error($"Error loading feature `{featureName}` from file `{_file.AsFileInfo().Name}`: {exception.Message}");
                ApiEx.Current.Logger.Error(exception.StackTrace);
                throw;
            }
        }

        /// <summary>
        ///     Saves the specified settings to file.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type" /> of object to parse the settings for the feature into.</typeparam>
        /// <param name="featureName">The name of the feature.</param>
        /// <param name="settings">The settings.</param>
        public void Save<T>(string featureName, T settings)
        {
            OnPropertyChanged(new FeatureSettingsChangedEventArgs<T>(featureName, settings));
        }

        private void OnPropertyChanged<T>(FeatureSettingsChangedEventArgs<T> args)
        {
            var json = _file.ParseAs<JObject>();
            var featureObj = json.SelectToken($"$.Features.{args.FeatureName}");
            if (featureObj is null)
            {
                json.SelectToken("$.Features")[args.FeatureName] = JToken.FromObject(args.FeatureSettings);
            }
            else
            {
                featureObj.Replace(JToken.FromObject(args.FeatureSettings));
            }
            _file.SaveFrom(json.ToString(Formatting.Indented));
        }
    }
}