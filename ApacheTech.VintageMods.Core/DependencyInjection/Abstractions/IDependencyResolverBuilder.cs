using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.DependencyInjection.Abstractions
{
    internal interface IDependencyResolverBuilder
    {
        /// <summary>
        ///     Registers services within the DI container.
        /// </summary>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public IDependencyResolverBuilder Configure(System.Action<IServiceCollection> callback);

        /// <summary>
        ///     Registers the game's API within the DI container.
        /// </summary>
        /// <param name="api">The game's core API.</param>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public IDependencyResolverBuilder RegisterAPI(ICoreAPI api);

        /// <summary>
        ///     Builds the specified DI container.
        /// </summary>
        public IDependencyResolver Build();
    }
}