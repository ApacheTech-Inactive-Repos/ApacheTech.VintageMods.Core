using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Services.Network
{
    /// <summary>
    ///     Provides narrowed scope access to network channels within the game.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface INetworkService
    {
        /// <summary>
        ///     Retrieves a client-side network channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>An instance of <see cref="IClientNetworkChannel"/>, used to send and receive network messages on the client.</returns>
        public IClientNetworkChannel ClientChannel(string channelName);

        /// <summary>
        ///     Retrieves the mod's default server-side network channel.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkChannel"/>, used to send and receive network messages on the server.</returns>
        public IServerNetworkChannel DefaultServerChannel { get; }

        /// <summary>
        ///     Retrieves the mod's default client-side network channel.
        /// </summary>
        /// <returns>An instance of <see cref="IClientNetworkChannel"/>, used to send and receive network messages on the client.</returns>
        public IClientNetworkChannel DefaultClientChannel { get; }

        /// <summary>
        ///     Retrieves the shared server-side network channel, between all VintageMods mods.
        /// </summary>
        /// <returns>An instance of <see cref="IServerNetworkChannel"/>, used to send and receive network messages on the server.</returns>
        public IServerNetworkChannel CommonServerChannel { get; }

        /// <summary>
        ///     Retrieves the shared client-side network channel, between all VintageMods mods.
        /// </summary>
        /// <returns>An instance of <see cref="IClientNetworkChannel"/>, used to send and receive network messages on the client.</returns>
        public IClientNetworkChannel CommonClientChannel { get; }

        /// <summary>
        ///     Retrieves a server-side network channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>An instance of <see cref="IServerNetworkChannel"/>, used to send and receive network messages on the server.</returns>
        public IServerNetworkChannel ServerChannel(string channelName);

        /// <summary>
        ///     Registers a network channel on the app side this method is called from.
        /// </summary>
        /// <param name="channelName">The name of the channel to register.</param>
        void RegisterChannel(string channelName);
    }
}