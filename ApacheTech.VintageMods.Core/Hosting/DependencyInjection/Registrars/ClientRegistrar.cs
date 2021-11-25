using ApacheTech.Common.Extensions.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Abstractions;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registrars
{
    /// <summary>
    ///     Handles registration of the Game's API within the client-side IOC Container.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
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
            services.RegisterSingleton(ApiEx.Client);
            services.RegisterSingleton(ApiEx.Client.World);
            services.RegisterSingleton(ApiEx.ClientMain);
            services.RegisterSingleton((ICoreAPICommon)ApiEx.Current);
            services.RegisterSingleton(ApiEx.Current);
        }
    }
}