using System;
using JetBrains.Annotations;
using SmartAssembly.Attributes;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions
{
    /// <summary>
    ///     An IOC Container, which holds references to registered types of services, and their instances.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IServiceCollection
    {
        /// <summary>
        ///     Registers a raw service descriptor, pre-populated with meta-data for the service.
        /// </summary>
        /// <param name="descriptor">The pre-populated descriptor for the service to register.</param>
        /// <seealso cref="ServiceDescriptor"/>
        /// 
        void Register(ServiceDescriptor descriptor);

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        /// 
        void RegisterSingleton<TService>() where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void RegisterSingleton<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void RegisterSingleton<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. Only one instance of the service will be created within the container.
        /// </summary>
        /// <typeparam name="TService">The instance to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Singleton"/>
        void RegisterSingleton<TService>(TService implementation);

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void RegisterTransient<TService>() where TService : class;

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <typeparam name="TImplementation">The type of implementation to use.</typeparam>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void RegisterTransient<TService, TImplementation>() where TImplementation : TService;

        /// <summary>
        ///     Registers a service as a singleton. A new instance of the service will be created each time it is resolved.
        /// </summary>
        /// <typeparam name="TService">The type of service to register.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <seealso cref="ServiceLifetime.Transient"/>
        void RegisterTransient<TService>(Func<IServiceResolver, TService> implementationFactory) where TService : class;

        /// <summary>
        ///     Build a service resolver, to access services within this collection.
        /// </summary>
        /// <returns>An IOC Service Resolver.</returns>
        IServiceResolver Build();
    }
}