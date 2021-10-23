using System;
using Microsoft.Extensions.DependencyInjection;

namespace ApacheTech.VintageMods.Core.Common.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        ///     Instantiate a type with constructor arguments provided directly and/or through the IOC Container.
        ///     Required a Constructor decorated with a <see cref="ActivatorUtilitiesConstructorAttribute"/> attribute.
        /// </summary>
        /// <typeparam name="TType">The type of object to instantiate.</typeparam>
        /// <param name="provider">The IOC provider to use to resolve dependencies.</param>
        /// <param name="args">Optional parameter arguments to pass to the constructor.</param>
        /// <returns>An activated object of type <typeparamref name="TType"/>, in the order the appear within the constructor signature.</returns>
        public static TType CreateInstance<TType>(this IServiceProvider provider, params object[] args)
        {
            return ActivatorUtilities.CreateInstance<TType>(provider, args);
        }
    }
}
