using ApacheTech.Common.Extensions.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registrars
{
    /// <summary>
    ///     Handles registration of the Game's API within the server-side IOC Container.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class ServerRegistrar : IRegistrar
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="ServerRegistrar"/> class.
        /// </summary>
        public static ServerRegistrar CreateInstance()
        {
            return new ServerRegistrar();
        }

        /// <summary>
        ///     Registers the Game's API within the server-side IOC Container.
        /// </summary>
        /// <param name="api">The server-side API.</param>
        /// <param name="container">The IOC container.</param>
        public void Register(ICoreAPI api, IServiceCollection container)
        {
            container.RegisterSingleton(ApiEx.Server);
            container.RegisterSingleton(ApiEx.Server.World);
            container.RegisterSingleton(ApiEx.ServerMain);
            container.RegisterSingleton((ICoreAPICommon)ApiEx.Current);
            container.RegisterSingleton(ApiEx.Current);
        }
    }
}