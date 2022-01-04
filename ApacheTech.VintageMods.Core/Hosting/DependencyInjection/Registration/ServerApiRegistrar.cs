using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using SmartAssembly.Attributes;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration
{
    /// <summary>
    ///     Handles registration of the Game's API within the server-side IOC Container.
    /// </summary>
    [DoNotPruneType]
    internal static class ServerApiRegistrar
    {
        /// <summary>
        ///     Registers the Game's API within the server-side IOC Container.
        /// </summary>
        /// <param name="container">The IOC container.</param>
        public static void RegisterServerApiEndpoints(IServiceCollection container)
        {
            container.RegisterSingleton(ApiEx.Server);
            container.RegisterSingleton(ApiEx.Server.World);
            container.RegisterSingleton(ApiEx.ServerMain);
            container.RegisterSingleton((ICoreAPICommon)ApiEx.Current);
            container.RegisterSingleton(ApiEx.Current);
        }
    }
}