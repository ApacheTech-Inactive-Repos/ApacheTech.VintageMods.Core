using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.Abstractions
{
    /// <summary>
    ///     Registers the Game's API within the IOC Container for a given app-side.
    /// </summary>
    public interface IRegistrar
    {
        /// <summary>
        ///     Registers the Game's API within the IOC Container for a given app-side.
        /// </summary>
        /// <param name="api">The app-side's API.</param>
        /// <param name="container">The IOC container.</param>
        void Register(ICoreAPI api, IServiceCollection container);
    }
}