using JetBrains.Annotations;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Common.Extensions.Game
{
    /// <summary>
    ///     Extension methods for when working with players and player entities.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class PlayerExtensions
    {
        /// <summary>
        ///     Sends a generic notification message to a given player, from the server.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="chatType">The type of message to send.</param>
        public static void SendMessage(this IServerPlayer player, string message, EnumChatType chatType = EnumChatType.Notification)
        {
            player.SendMessage(GlobalConstants.CurrentChatGroup, message, chatType);
        }
    }
}