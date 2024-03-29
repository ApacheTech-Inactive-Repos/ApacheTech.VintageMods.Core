﻿using System;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Annotation.Attributes;
using ApacheTech.VintageMods.Core.Extensions.Game;
using ApacheTech.VintageMods.Core.Services.Network.Extensions;
using ApacheTech.VintageMods.Core.Services.Network.Packets;
using SmartAssembly.Attributes;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Services.Network
{
    /// <summary>
    ///     A service that provides narrowed scope access to network channels within the game.
    /// </summary>#
    /// <remarks>
    ///     Adds two channels, by default; one called "VintageMods", which can be
    ///     used as an IPC pipe between VintageMods mods, and one for with the ModId
    ///     of the calling Mod. Other channels can be registered, as needed.
    /// </remarks>
    /// <seealso cref="INetworkService" />
    [RegisteredService(ServiceLifetime.Singleton, typeof(INetworkService))]
    [DoNotPruneType]
    internal class NetworkService : INetworkService
    {
        private readonly ICoreClientAPI _capi;
        private readonly ICoreServerAPI _sapi;
        private readonly string _modId;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="NetworkService"/> class.
        /// </summary>
        /// <param name="api">The universal Core API.</param>
        /// <param name="modInfo">The <see cref="VintageModInfoAttribute"/> of the mod that is using this service.</param>
        public NetworkService(ICoreAPI api, IVintageModInfo modInfo)
        {
            _modId = modInfo.ModId;
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    _sapi = api.ForServer();
                    ServerChannel("VintageMods").RegisterPropertyPacketTypes();
                    ServerChannel(_modId)
                        .RegisterPropertyPacketTypes()
                        .RegisterMessageType<WorldNamePacket>()
                        .SetMessageHandler<WorldNamePacket>((forPlayer, packet) =>
                        {
                            DefaultServerChannel.SendPacket(packet.With(p => p.Name = _sapi.WorldManager.SaveGame.WorldName), forPlayer);
                        });
                    break;
                case EnumAppSide.Client:
                    _capi = api.ForClient();
                    ClientChannel("VintageMods").RegisterPropertyPacketTypes();
                    ClientChannel(_modId).RegisterPropertyPacketTypes();
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        /// <summary>
        ///     Retrieves a client-side network channel.
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <returns>An instance of <see cref="IClientNetworkChannel" />, used to send and receive network messages on the client.</returns>
        public IClientNetworkChannel ClientChannel(string channelName)
        {
            var channel = _capi?.Network.GetChannel(channelName);
            try
            {
                return channel ?? _capi?.Network.RegisterChannel(channelName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Retrieves a server-side network channel.
        /// </summary>
        /// <param name="channelName">The name of the channel to register.</param>
        /// <returns>An instance of <see cref="IServerNetworkChannel" />, used to send and receive network messages on the server.</returns>
        public IServerNetworkChannel ServerChannel(string channelName)
        {
            var channel = _sapi?.Network.GetChannel(channelName);
            try
            {
                return channel ?? _sapi?.Network.RegisterChannel(channelName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Retrieves the mod's default server-side network channel.
        /// </summary>
        /// <value>The default server channel.</value>
        public IServerNetworkChannel DefaultServerChannel => ServerChannel(_modId);

        /// <summary>
        ///     Retrieves the mod's default client-side network channel.
        /// </summary>
        /// <value>The default client channel.</value>
        public IClientNetworkChannel DefaultClientChannel => ClientChannel(_modId);

        /// <summary>
        ///     Retrieves the shared server-side network channel, between all VintageMods mods.
        /// </summary>
        /// <value>The common server channel.</value>
        public IServerNetworkChannel CommonServerChannel => ServerChannel("VintageMods");

        /// <summary>
        ///     Retrieves the shared client-side network channel, between all VintageMods mods.
        /// </summary>
        /// <value>The common client channel.</value>
        public IClientNetworkChannel CommonClientChannel => ClientChannel("VintageMods");

        /// <summary>
        ///     Registers a network channel on the app side this method is called from.
        /// </summary>
        /// <param name="channelName">The name of the channel to register.</param>
        public void RegisterChannel(string channelName)
        {
            if (_capi is not null && _capi.Network.GetChannel(channelName) is null)
            {
                _capi.Network.RegisterChannel(channelName);
            }

            if (_sapi is not null && _sapi.Network.GetChannel(channelName) is null)
            {
                _sapi.Network.RegisterChannel(channelName);
            }
        }
    }
}
