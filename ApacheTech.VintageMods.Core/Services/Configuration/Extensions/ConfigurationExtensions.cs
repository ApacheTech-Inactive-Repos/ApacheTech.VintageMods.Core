using System;
using System.Linq;
using ApacheTech.VintageMods.Core.Hosting;
using ApacheTech.VintageMods.Core.Services.Configuration.Contracts;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApacheTech.VintageMods.Core.Services.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        private static T Build<T>(this IConfigurationBuilder builder) where T : class, IConfigurationRoot
        {
            var providers = builder.Sources.Select(source => source.Build(builder)).ToList();
            return (T)Activator.CreateInstance(typeof(T), providers);
        }

        public static IGlobalConfiguration BuildGlobalConfig(this IConfigurationBuilder builder)
        {
            return builder.Build<GlobalConfigurationRoot>();
        }

        public static IWorldConfiguration BuildWorldConfig(this IConfigurationBuilder builder)
        {
            return builder.Build<WorldConfigurationRoot>();
        }

        public static IConfigurationSection Feature(this IConfigurationRoot configuration, string featureName)
        {
            return configuration.GetSection("Features").GetSection(featureName);
        }

        public static T FeatureAs<T>(this IConfigurationRoot configuration, string featureName)
        {
            return configuration.GetSection("Features").GetSection(featureName).Get<T>();
        }

        public static void FeatureBind<T>(this IConfigurationRoot configuration, string featureName, T sectionObject)
        {
            configuration.GetSection("Features").GetSection(featureName).Bind(sectionObject);
        }

        private static IJsonModFile GetJsonFile(this IConfigurationRoot configuration)
        {
            var source = configuration.Providers.OfType<NewtonsoftJsonConfigurationProvider>().First().Source;
            var fileName = source.FileProvider.GetFileInfo(source.Path).Name;
            return ModServices.IOC.Resolve<IFileSystemService>().GetJsonFile(fileName);
        }

        public static void SaveFeature<T>(this IConfigurationRoot configuration, string featureName, T sectionObject)
        {
            var file = configuration.GetJsonFile();
            var jObject = file.ParseAs<JObject>();
            jObject["Features"][featureName] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            file.SaveFrom(JsonConvert.SerializeObject(jObject, Formatting.Indented));
            configuration.Reload();
        }
    }
}