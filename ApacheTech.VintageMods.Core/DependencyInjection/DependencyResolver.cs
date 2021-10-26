using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.VintageMods.Core.Common.Extensions;
using ApacheTech.VintageMods.Core.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.DependencyInjection
{
    internal class DependencyResolver : IDependencyResolver, IDisposable
    {
        /// <summary>
        ///     Gets the service provider, used to resolve instances of services within the DI Container.
        /// </summary>
        /// <value>The <see cref="IServiceProvider" /> service. provider.</value>
        private readonly IServiceProvider _provider;

        private readonly List<IDisposable> _disposingServices = new();
        
        internal DependencyResolver(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        ///     Instantiate a type with constructor arguments provided directly and/or through the IOC Container.
        ///     Required a Constructor decorated with a <see cref="ActivatorUtilitiesConstructorAttribute" /> attribute.
        /// </summary>
        /// <typeparam name="TService">The type of object to instantiate.</typeparam>
        /// <param name="args">Optional parameter arguments to pass to the constructor.</param>
        /// <returns>
        ///     An activated object of type <typeparamref name="TService" />, in the order the appear within the constructor
        ///     signature.
        /// </returns>
        public TService CreateInstance<TService>(params object[] args) where TService : class
        {
            var service = ActivatorUtilities.CreateInstance<TService>(_provider, args);
            return HandleDisposableInstances(service);
        }

        public TService Resolve<TService>()
        {
            var service = _provider.GetRequiredService<TService>();
            return HandleDisposableInstances(service);
        }

        private TService HandleDisposableInstances<TService>(TService service)
        {
            if (service.GetType().GetInterface(nameof(IDisposable)) is null) return service;
            _disposingServices.AddIfNotPresent(service as IDisposable);
            return service;
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
    }
}