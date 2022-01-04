using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration
{
    /// <summary>
    ///     Handles registration of the Game's API within the client-side IOC Container.
    /// </summary>
    [DoNotPruneType]
    internal static class ClientApiRegistrar
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