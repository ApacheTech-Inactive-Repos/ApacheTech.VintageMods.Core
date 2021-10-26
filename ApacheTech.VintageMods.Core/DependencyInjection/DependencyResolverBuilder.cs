using System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace ApacheTech.VintageMods.Core.DependencyInjection
{
    internal class DependencyResolverBuilder : IDependencyResolverBuilder
    {
        /// <summary>
        ///     Gets the DI container for the mod.
        /// </summary>
        /// <value>The <see cref="IServiceCollection" /> DI container.</value>
        private readonly IServiceCollection _container = new ServiceCollection();

        /// <summary>
        ///     Registers services within the DI container.
        /// </summary>
        /// <returns>Returns the same instance of <see cref="IDependencyResolver" /> that it was passed.</returns>
        public IDependencyResolverBuilder Configure(System.Action<IServiceCollection> callback)
        {
            callback(_container);
            return this;
        }

        /// <summary>
        ///     Builds the specified DI container.
        /// </summary>
        public IDependencyResolver Build()
        {
            return new DependencyResolver(_container.BuildServiceProvider());
        }

        public IDependencyResolverBuilder RegisterAPI(ICoreAPI api)
        {
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    ApiEx.Server = (ICoreServerAPI)api;
                    ApiEx.ServerMain = (ServerMain)ApiEx.Server.World;
                    _container.AddSingleton(ApiEx.Server);
                    _container.AddSingleton(ApiEx.Server.World);
                    _container.AddSingleton(ApiEx.ServerMain);
                    _container.AddSingleton((ICoreAPICommon)ApiEx.Current);
                    _container.AddSingleton(ApiEx.Current);
                    break;
                case EnumAppSide.Client:
                    ApiEx.Client = (ICoreClientAPI)api;
                    ApiEx.ClientMain = (ClientMain)ApiEx.Client.World;
                    _container.AddSingleton(ApiEx.Client);
                    _container.AddSingleton(ApiEx.Client.World);
                    _container.AddSingleton(ApiEx.ClientMain);
                    _container.AddSingleton((ICoreAPICommon)ApiEx.Current);
                    _container.AddSingleton(ApiEx.Current);
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }

    }
}