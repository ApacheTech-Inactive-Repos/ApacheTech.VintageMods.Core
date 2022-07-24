using System;
using System.Collections.Generic;
using System.ComponentModel;
using ApacheTech.Common.Extensions.System;
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
        private readonly List<IDisposable> _observers;

        public IJsonModFile File { get; }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonSettingsFile"/> class.
        /// </summary>
        /// <param name="file">The underlying file, registered within the file system service.</param>
        private JsonSettingsFile(IJsonModFile file)
        {
            File = file;
            _observers = new List<IDisposable>();
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="JsonSettingsFile"/> class.
        /// </summary>
        /// <param name="file">The underlying file, registered within the file system service.</param>
        public static JsonSettingsFile FromJsonFile(IJsonModFile file)
        {
            return new JsonSettingsFile(file);
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
        public T Feature<T>(string featureName = null) where T: class, new()
        {
            featureName ??= typeof(T).Name.Replace("Settings", "");
            try
            {
                var json = File.ParseAs<JObject>();
                if (json is null)
                {
                    var defaultData = new T();
                    Save(json = JObject.FromObject(defaultData), featureName);
                }
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
                _observers.AddIfNotPresent(observer);
                observer.PropertyChanged += OnPropertyChanged;
                return featureObj;
            }
            catch (Exception exception)
            {
                ApiEx.Current.Logger.Error($"Error loading feature `{featureName}` from file `{File.AsFileInfo().Name}`: {exception.Message}");
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
        [Obsolete]
        public void Save<T>(string featureName, T settings)
        {
            OnPropertyChanged(new FeatureSettingsChangedEventArgs<T>(featureName, settings));
        }

        /// <summary>
        ///     Saves the specified settings to file.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type" /> of object to parse the settings for the feature into.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <param name="featureName">The name of the feature.</param>
        public void Save<T>(T settings, string featureName = null)
        {
            featureName ??= typeof(T).Name.Replace("Settings", "");
            OnPropertyChanged(new FeatureSettingsChangedEventArgs<T>(featureName, settings));
        }

        private void OnPropertyChanged<T>(FeatureSettingsChangedEventArgs<T> args)
        {
            var json = File.ParseAs<JObject>() ?? JObject.Parse("{ \"Features\": {  } }");
            var featureObj = json.SelectToken($"$.Features.{args.FeatureName}");
            if (featureObj is null)
            {
                json.SelectToken("$.Features")[args.FeatureName] = JToken.FromObject(args.FeatureSettings);
            }
            else
            {
                featureObj.Replace(JToken.FromObject(args.FeatureSettings));
            }
            File.SaveFrom(json.ToString(Formatting.Indented));
        }

        public void Dispose()
        {
            _observers.Purge();
        }
    }
}