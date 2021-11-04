using System;
using JetBrains.Annotations;
using SmartAssembly.Attributes;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions
{
    /// <summary>
    ///     Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IServiceResolver : IDisposable
    {
        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
        public object GetService(Type serviceType);

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if no object of type <paramref name="serviceType" /> can be instantiated from the service collection.</returns>
        public object CreateInstance(Type serviceType, params object[] args);

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="args">An optional list of arguments, sent the the constructor of the instantiated class.</param>
        /// <returns>An object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no object of type <typeparamref name="T" /> can be instantiated from the service collection.</returns>
        T CreateInstance<T>(params object[] args) where T : class;

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no service object of type <typeparamref name="T" />.</returns>
        T Resolve<T>();
    }
}