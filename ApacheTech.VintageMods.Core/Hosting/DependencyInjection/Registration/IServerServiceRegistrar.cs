using System;
using ApacheTech.Common.DependencyInjection.Abstractions;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration
{
    /// <summary>
    ///     Represents a class that can add services to the Server IOC container. Implements <see cref="IDisposable" />.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IServerServiceRegistrar : IDisposable
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        void ConfigureServerModServices(IServiceCollection services);

        /// <summary>
        ///     Called on the server, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="sapi">
        ///     The core API implemented by the server.
        ///     The main interface for accessing the server.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        void StartPreServerSide(ICoreServerAPI sapi);

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