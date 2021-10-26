using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.DependencyInjection.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace ApacheTech.VintageMods.Core.DependencyInjection.Registrars
{
    /// <summary>
    ///     Handles registration of the Game's API within the client-side IOC Container.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class ClientRegistrar : IRegistrar
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="ClientRegistrar"/> class.
        /// </summary>
        public static ClientRegistrar CreateInstance()
        {
            return new ClientRegistrar();
        }

        /// <summary>
        ///     Registers the Game's API within the client-side IOC Container.
        /// </summary>
        /// <param name="api">The client-side API.</param>
        /// <param name="container">The IOC container.</param>
        public void Register(ICoreAPI api, IServiceCollection container)
        {
            ApiEx.Client = (ICoreClientAPI)api;
            ApiEx.ClientMain = (ClientMain)ApiEx.Client.World;
            container.AddSingleton(ApiEx.Client);
            container.AddSingleton(ApiEx.Client.World);
            container.AddSingleton(ApiEx.ClientMain);
            container.AddSingleton((ICoreAPICommon)ApiEx.Current);
            container.AddSingleton(ApiEx.Current);
        }
    }
}