﻿using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    /// <summary>
    ///     Extension methods for when working with players and player entities.
    /// </summary>
    public static class PlayerExtensions
    {
        /// <summary>
        ///     Sends a generic notification message to a given player, from the server.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="groupId">The chat group to send the message to.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="chatType">The type of message to send.</param>
        public static void SendMessage(this IPlayer player, int groupId, string message, EnumChatType chatType = EnumChatType.Notification)
        {
            if (player is IClientPlayer clientPlayer)
            {
                clientPlayer.ShowChatNotification(message);
                return;
            }
            ((IServerPlayer)player).SendMessage(groupId, message, chatType);
        }

        /// <summary>
        ///     Sends a message to the player, giving feedback about an invalid syntax message.
        /// </summary>
        /// <param name="player">The player to send the message to.</param>
        /// <param name="groupId">The chat group to send the message to.</param>
        public static void SendInvalidSyntaxMessage(this IPlayer player, int groupId)
        {
            player.SendMessage(groupId, Lang.Get("vmods:errors.invalid-command-syntax"), EnumChatType.CommandError);
        }
    }
}