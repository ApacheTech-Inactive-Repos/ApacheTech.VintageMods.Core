using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using JetBrains.Annotations;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registrars
{
    /// <summary>
    ///     Handles registration of the Game's API within the client-side IOC Container.
    /// </summary>
    [DoNotPruneType, UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal sealed class ClientApiRegistrar
    {
        /// <summary>
        ///     Registers the Game's API within the client-side IOC Container.
        /// </summary>
        /// <param name="services">The IOC container.</param>
        public static void RegisterClientApiEndpoints(IServiceCollection services)
        {
            services.RegisterSingleton(ApiEx.Client);
            services.RegisterSingleton(ApiEx.Client.World);
            services.RegisterSingleton(ApiEx.ClientMain);
            services.RegisterSingleton((ICoreAPICommon)ApiEx.Current);
            services.RegisterSingleton(ApiEx.Current);
        }
    }
}