using ApacheTech.VintageMods.Core.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.DependencyInjection
{
    /// <summary>
    ///     Builds an IOC Dependency Resolver for the Client or Server.
    /// </summary>
    /// <seealso cref="IDependencyResolverBuilder" />
    internal class DependencyResolverBuilder : IDependencyResolverBuilder
    {
        /// <summary>
        ///     Gets the DI container for the mod.
        /// </summary>
        /// <value>The <see cref="IServiceCollection" /> DI container.</value>
        private readonly IServiceCollection _container = new ServiceCollection();
        
        /// <summary>
        ///     Registers services within the DI container.
        /// </summary>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public IDependencyResolverBuilder Configure(System.Action<IServiceCollection> callback)
        {
            callback(_container);
            return this;
        }

        /// <summary>
        ///     Builds the specified DI container.
        /// </summary>
        public IDependencyResolver Build()
        {
            return new DependencyResolver(_container.BuildServiceProvider());
        }

        /// <summary>
        ///     Registers the game's API within the DI container.
        /// </summary>
        /// <param name="api">The game's core API.</param>
        /// <param name="registrar">The registrar to use, to register the API for the current app-side.</param>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public IDependencyResolverBuilder RegisterAPI(ICoreAPI api, IRegistrar registrar)
        {
            registrar.Register(api, _container);
            return this;
        }
    }
}