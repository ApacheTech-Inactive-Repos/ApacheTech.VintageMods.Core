using Vintagestory.API.Client;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class NetworkExtensions
    {
        public static void SendPacket<T>(this IClientNetworkChannel channel) where T : new()
        {
            channel.SendPacket(new T());
        }

        public static void SendPacket<T>(this IServerNetworkChannel channel) where T : new()
        {
            channel.SendPacket(new T());
        }
    }
}