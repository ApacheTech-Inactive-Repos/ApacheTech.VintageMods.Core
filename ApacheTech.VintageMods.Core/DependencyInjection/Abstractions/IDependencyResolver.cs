using System;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.DependencyInjection.Abstractions
{
    /// <summary>
    ///     Resolves dependencies within the IOC Container, for a given app-side.
    /// </summary>
    public interface IDependencyResolver
    {
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
        public TService CreateInstance<TService>(params  object[] args) where TService : class;

        /// <summary>
        ///     Get a service of type <see cref="TService" />, from the underlying <see cref="IServiceProvider" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service object to get.</typeparam>
        /// <returns>A service object of type <see cref="TService" /></returns>
        public TService Resolve<TService>();
    }
}