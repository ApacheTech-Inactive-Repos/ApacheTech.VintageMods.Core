using ApacheTech.VintageMods.Core.Common.Extensions;
using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Services.Network
{
    /// <summary>
    ///     A service that provides narrowed scope access to network channels within the game.
    /// </summary>
    /// <seealso cref="INetworkService" />
    internal class NetworkService : INetworkService
    {
        private readonly ICoreAPI _api;
        [CanBeNull] private readonly ICoreClientAPI _capi;
        [CanBeNull] private readonly ICoreServerAPI _sapi;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="NetworkService"/> class.
        /// </summary>
        /// <param name="api">The API.</param>
        public NetworkService(ICoreAPI api)
        {
            _api = api;
            _capi = api.ForClient();
            _sapi = api.ForServer();
        }

        /// <summary>
        ///     Retrieves a client-side network channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>An instance of <see cref="IClientNetworkChannel" />, used to send and receive network messages on the client.</returns>
        public IClientNetworkChannel ClientChannel(string channelName)
        {
            return _capi?.Network.GetChannel(channelName);
        }

        /// <summary>
        ///     Retrieves a server-side network channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>An instance of <see cref="IServerNetworkChannel" />, used to send and receive network messages on the server.</returns>
        public IServerNetworkChannel ServerChannel(string channelName)
        {
            return _sapi?.Network.GetChannel(channelName);
        }

        /// <summary>
        ///     Registers a network channel on the app side this method is called from.
        /// </summary>
        /// <param name="channelName">The name of the channel to register.</param>
        public void RegisterChannel(string channelName)
        {
            _capi?.Network.RegisterChannel(channelName);
            _sapi?.Network.RegisterChannel(channelName);
        }
    }
}
