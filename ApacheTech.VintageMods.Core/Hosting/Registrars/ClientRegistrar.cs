using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.Registrars
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
        /// <param name="services">The IOC container.</param>
        public void Register(ICoreAPI api, IServiceCollection services)
        {
            services.AddSingleton(ApiEx.Client);
            services.AddSingleton(ApiEx.Client.World);
            services.AddSingleton(ApiEx.ClientMain);
            services.AddSingleton((ICoreAPICommon)ApiEx.Current);
            services.AddSingleton(ApiEx.Current);
        }
    }
}