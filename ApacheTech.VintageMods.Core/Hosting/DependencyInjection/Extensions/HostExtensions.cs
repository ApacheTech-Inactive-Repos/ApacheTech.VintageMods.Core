using System;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Annotation;
using JetBrains.Annotations;
using Vintagestory.API.Common;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions
{
    public static class HostExtensions
    {
        /// <summary>
        ///     Determines whether a constructor is decorated with a <see cref="SidedServiceProviderConstructorAttribute"/> attribute that matched the current app-side.
        /// </summary>
        /// <param name="constructor">The constructor to check.</param>
        /// <returns><c>true</c> if the dependencies for the constructor should be resolved via the service provider, <c>false</c> otherwise.</returns>
        public static bool IOCEnabled(this ConstructorInfo constructor)
        {
            return constructor
                .GetCustomAttributes(typeof(SidedServiceProviderConstructorAttribute), false)
                .Cast<SidedServiceProviderConstructorAttribute>()
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
        [CanBeNull]
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
    }
}
