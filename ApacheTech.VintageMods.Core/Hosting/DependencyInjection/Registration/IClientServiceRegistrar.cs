using System;
using ApacheTech.Common.DependencyInjection.Abstractions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration
{
    /// <summary>
    ///     Represents a class that can add services to the Client IOC container. Implements <see cref="IDisposable" />.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IClientServiceRegistrar : IDisposable
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        void ConfigureClientModServices(IServiceCollection services);

        /// <summary>
        ///     Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">
        ///     The core API implemented by the client.
        ///     The main interface for accessing the client.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        void StartPreClientSide(ICoreClientAPI capi);

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        void Start(ICoreAPI api);
    }
}