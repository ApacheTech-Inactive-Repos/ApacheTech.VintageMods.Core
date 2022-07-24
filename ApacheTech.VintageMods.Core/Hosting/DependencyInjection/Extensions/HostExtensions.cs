using System;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation;
using Vintagestory.API.Common;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        ///     Determines whether a constructor is decorated with a <see cref="SidedConstructorAttribute"/> attribute that matched the current app-side.
        /// </summary>
        /// <param name="constructor">The constructor to check.</param>
        /// <returns><c>true</c> if the dependencies for the constructor should be resolved via the service provider, <c>false</c> otherwise.</returns>
        public static bool IOCEnabled(this ConstructorInfo constructor)
        {
            return constructor
                .GetCustomAttributes(typeof(SidedConstructorAttribute), false)
                .Cast<SidedConstructorAttribute>()
                .Any(q => q.Side == EnumAppSide.Universal || q.Side == ApiEx.Side);
        }

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <param name="provider">The service provider to use to resolve dependencies for the instantiated class.</param>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if no object of type <paramref name="serviceType" /> can be instantiated from the service collection.</returns>
        public static object CreateSidedInstance(this IServiceResolver provider, Type serviceType, params object[] args)
        {
            return ActivatorEx.CreateInstance(provider as IServiceProvider, serviceType, args);
        }

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="provider">The service provider to use to resolve dependencies for the instantiated class.</param>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>An object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no object of type <typeparamref name="T" /> can be instantiated from the service collection.</returns>
        public static T CreateSidedInstance<T>(this IServiceResolver provider, params object[] args) where T : class
        {
            return (T)CreateSidedInstance(provider, typeof(T), args);
        }

        /// <summary>
        ///     Registers all <see cref="ModSystem"/>s in the current mod, into the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the <see cref="ModSystem"/>s to.</param>
        public static void AddModSystems(this IServiceCollection services)
        {
            var modSystems = AssemblyEx
                .GetModAssembly()
                .GetTypes()
                .Where(p => p.IsSubclassOf(typeof(ModSystem)) && !p.IsAbstract);

            foreach (var type in modSystems)
            {
                var system = ApiEx.Current.ModLoader.GetModSystem(type.FullName);
                services.Add(new ServiceDescriptor(type, system, ServiceLifetime.Singleton));
            }
        }

        /// <summary>
        ///     Registers a client side features into the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the features to.</param>
        public static void AddModSystem<T>(this IServiceCollection services) where T : ModSystem
        {
            services.AddSingleton(_ => ApiEx.Current.ModLoader.GetModSystem<T>());
        }

        public static void AddFeatureWorldSettings<TSettings>(this IServiceCollection services, string featureName = null) where TSettings : class, new()
        {
            if (string.IsNullOrWhiteSpace(featureName)) featureName = typeof(TSettings).Name.Replace("Settings", "");
            services.AddSingleton(_ =>
            {
                var instance = ModSettings.World.Feature<TSettings>(featureName);
                return instance;
            });
        }

        public static void AddFeatureGlobalSettings<TSettings>(this IServiceCollection services, string featureName = null) where TSettings : class, new()
        {
            if (string.IsNullOrWhiteSpace(featureName)) featureName = typeof(TSettings).Name.Replace("Settings", "");
            services.AddSingleton(_ => ModSettings.Global.Feature<TSettings>(featureName));
        }

        public static void AddFeatureLocalSettings<TSettings>(this IServiceCollection services, string featureName = null) where TSettings : class, new()
        {
            if (string.IsNullOrWhiteSpace(featureName)) featureName = typeof(TSettings).Name.Replace("Settings", "");
            services.AddSingleton(_ => ModSettings.Local.Feature<TSettings>(featureName));
        }
    }
}
