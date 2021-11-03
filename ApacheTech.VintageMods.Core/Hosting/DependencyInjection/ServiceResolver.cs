﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.VintageMods.Core.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions;
using JetBrains.Annotations;
using SmartAssembly.Attributes;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection
{
    /// <summary>
    ///     Defines a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ServiceResolver : IServiceResolver
    {
        private readonly List<ServiceDescriptor> _serviceDescriptors;
        private readonly List<IDisposable> _disposingServices = new();

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServiceResolver"/> class.
        /// </summary>
        /// <param name="serviceDescriptors">The service descriptors.</param>
        internal ServiceResolver(List<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no service object of type <paramref name="serviceType" />.</returns>
        [CanBeNull]
        public object GetService(Type serviceType)
        {
            var descriptor = _serviceDescriptors.SingleOrDefault(p => p.ServiceType == serviceType);

            if (descriptor == null)
            {
                throw new Exception($"No service of type {serviceType.Name} has been registered.");
            }

            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }
            
            var implementationType = descriptor.ImplementationType ?? descriptor.ServiceType;
            if (implementationType.IsAbstract)
            {
                throw new Exception("Cannot instantiate abstract classes.");
            }
            if (implementationType.IsInterface)
            {
                throw new Exception("Cannot instantiate interfaces");
            }

            var implementation = CreateInstance(implementationType);
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.Implementation = implementation;
            }
            return implementation;
        }

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if no object of type <paramref name="serviceType" /> can be instantiated from the service collection.</returns>
        [CanBeNull]
        public object CreateInstance(Type serviceType)
        {
            var constructorInfo = serviceType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();
            var service =  Activator.CreateInstance(serviceType, parameters);
            return HandleDisposableInstances(service);
        }

        /// <summary>
        ///     Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <returns>A service object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no service object of type <typeparamref name="T" />.</returns>
        public T Resolve<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        ///     Creates an object of a specified type, using the IOC Container to resolve dependencies.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>An object of type <typeparamref name="T" />.
        /// 
        /// -or-
        /// 
        /// <see langword="null" /> if there is no object of type <typeparamref name="T" /> can be instantiated from the service collection.</returns>
        public T CreateInstance<T>() where T : class
        {
            return (T)CreateInstance(typeof(T));
        }

        /// <summary>
        ///     Disposes all services that require disposing.
        /// </summary>
        public void Dispose()
        {
            foreach (var service in _disposingServices.Where(service => service is not null))
            {
                service.Dispose();
            }
            _disposingServices.Clear();
        }

        private TService HandleDisposableInstances<TService>(TService service)
        {
            if (service.GetType().GetInterface(nameof(IDisposable)) is null) return service;
            _disposingServices.AddIfNotPresent(service as IDisposable);
            return service;
        }
    }
}