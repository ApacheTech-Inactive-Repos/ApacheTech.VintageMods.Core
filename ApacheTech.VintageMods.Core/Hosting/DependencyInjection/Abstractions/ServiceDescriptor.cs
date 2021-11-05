using System;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions
{
    /// <summary>
    ///     Describes a service with its service type, implementation, and lifetime.
    /// </summary>
    public class ServiceDescriptor
    {
        /// <summary>
        ///     Gets the type of the service.
        /// </summary>
        /// <value>The <see cref="Type"/> of the service.</value>
        public Type ServiceType { get; }

        /// <summary>
        ///     Gets the type of the implementation.
        /// </summary>
        /// <value>The <see cref="Type"/> of the implementation of the service.</value>
        public Type ImplementationType { get; }

        /// <summary>
        ///     Gets the concrete implementation, which gets returned to the user.
        /// </summary>
        /// <value>The implementation of the service.</value>
        public object Implementation { get; internal set; }

        /// <summary>
        /// Specifies the lifetime of a service in an <see cref="IServiceCollection" />.
        /// </summary>
        /// <value>The <see cref="ServiceLifetime"/> of the service.</value>
        public ServiceLifetime Lifetime { get; } 
        
        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="implementation">The concrete implementation, which gets returned to the user.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        public ServiceDescriptor(object implementation, ServiceLifetime lifetime)
        {
            ServiceType = implementation.GetType();
            Implementation = implementation;
            ImplementationType = implementation.GetType();
            Lifetime = lifetime;
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        public ServiceDescriptor(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        /// <summary>
        ///     Initialises a new instance of <see cref="ServiceDescriptor"/> with the specified <paramref name="implementationType"/>.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="implementationType">The <see cref="Type"/> implementing the service.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            ImplementationType = implementationType;
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServiceDescriptor"/> class.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the service.</param>
        /// <param name="implementation">The concrete implementation, which gets returned to the user.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        public ServiceDescriptor(Type serviceType, object implementation, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
            ImplementationType = implementation.GetType();
            Implementation = implementation;
        }
    }
}