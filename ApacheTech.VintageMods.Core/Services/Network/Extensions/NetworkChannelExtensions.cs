using System;
using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Extensions.Reflection;
using ApacheTech.VintageMods.Core.Services.Network.Packets;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace ApacheTech.VintageMods.Core.Services.Network.Extensions
{
    public static class NetworkChannelExtensions
    {
        public static TChannel RegisterPropertyPacketTypes<TChannel>(this TChannel channel)
            where TChannel : INetworkChannel
        {
            return (TChannel)channel
                .RegisterMessageType<SetPropertyPacket<bool>>()
                .RegisterMessageType<SetPropertyPacket<byte>>()
                .RegisterMessageType<SetPropertyPacket<sbyte>>()
                .RegisterMessageType<SetPropertyPacket<char>>()
                .RegisterMessageType<SetPropertyPacket<decimal>>()
                .RegisterMessageType<SetPropertyPacket<double>>()
                .RegisterMessageType<SetPropertyPacket<int>>()
                .RegisterMessageType<SetPropertyPacket<uint>>()
                .RegisterMessageType<SetPropertyPacket<long>>()
                .RegisterMessageType<SetPropertyPacket<ulong>>()
                .RegisterMessageType<SetPropertyPacket<short>>()
                .RegisterMessageType<SetPropertyPacket<ushort>>()
                .RegisterMessageType<SetPropertyPacket<string>>();
        }

        public static IClientNetworkChannel UnsetMessageHandler<T>(this IClientNetworkChannel channel)
        {
            var concrete = (Vintagestory.Client.NoObf.NetworkChannel)channel;
            var messageTypes = concrete.GetField<Dictionary<Type, int>>("messageTypes");
            var typeFromHandle = typeof(T);
            if (!messageTypes.TryGetValue(typeof(T), out var index))
            {
                throw new KeyNotFoundException($"No such message type {typeFromHandle} registered. Did you forgot to call RegisterMessageType?");
            }
            if (typeof(T).IsArray)
            {
                throw new ArgumentException("Please do not use array messages, they seem to cause serialisation problems in rare cases. Pack that array into its own class.");
            }
            var handlers = concrete.GetField<Action<object>[]>("handlers");
            handlers.RemoveEntry(index);
            return channel;
        }

        public static IServerNetworkChannel UnsetMessageHandler<T>(this IServerNetworkChannel channel)
        {
            var concrete = (Vintagestory.Server.NetworkChannel)channel;
            var messageTypes = concrete.GetField<Dictionary<Type, int>>("messageTypes");
            var typeFromHandle = typeof(T);
            if (!messageTypes.TryGetValue(typeof(T), out var index))
            {
                throw new KeyNotFoundException($"No such message type {typeFromHandle} registered. Did you forgot to call RegisterMessageType?");
            }
            if (typeof(T).IsArray)
            {
                throw new ArgumentException("Please do not use array messages, they seem to cause serialisation problems in rare cases. Pack that array into its own class.");
            }
            var handlers = concrete.GetField<Action<object>[]>("handlers");
            handlers.RemoveEntry(index);
            return channel;
        }
    }
}
