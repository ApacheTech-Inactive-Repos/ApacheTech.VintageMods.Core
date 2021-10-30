using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.Registrars
{
    /// <summary>
    ///     Handles registration of the Game's API within the server-side IOC Container.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
            container.AddSingleton(ApiEx.Server);
            container.AddSingleton(ApiEx.Server.World);
            container.AddSingleton(ApiEx.ServerMain);
            container.AddSingleton((ICoreAPICommon)ApiEx.Current);
            container.AddSingleton(ApiEx.Current);
        }
    }
}