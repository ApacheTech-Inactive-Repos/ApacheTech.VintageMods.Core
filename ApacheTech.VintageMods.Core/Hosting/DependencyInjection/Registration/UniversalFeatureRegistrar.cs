using ApacheTech.Common.DependencyInjection.Abstractions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration
{
    /// <summary>
    ///     Acts as a base class for all features that need to register services within the IOC Container, on the Client.
    /// </summary>
    public abstract class UniversalFeatureRegistrar : IClientServiceRegistrar, IServerServiceRegistrar
    {
        /// <summary>
        /// Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public abstract void ConfigureClientModServices(IServiceCollection services);

        /// <summary>
        /// Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public abstract void ConfigureServerModServices(IServiceCollection services);

        /// <summary>
        /// Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">The core API implemented by the client.
        /// The main interface for accessing the client.
        /// Contains all sub-components, and some miscellaneous methods.</param>
        public virtual void StartPreClientSide(ICoreClientAPI capi) { }

        /// <summary>
        /// Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="sapi">The core API implemented by the server.
        /// The main interface for accessing the server.
        /// Contains all sub-components, and some miscellaneous methods.</param>
        public virtual void StartPreServerSide(ICoreServerAPI sapi) { }

        /// <summary>
        /// Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">Common API Components that are available on the server and the client.
        /// Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.</param>
        public virtual void Start(ICoreAPI api) { }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose() { }
    }
}