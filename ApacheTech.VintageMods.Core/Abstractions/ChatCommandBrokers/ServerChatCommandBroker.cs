using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Abstractions.ChatCommandBrokers
{
    /// <summary>
    ///     Delegates the handling of server-side chat commands, mapped through an internal dictionary, indexed by the name of the command.
    /// </summary>
    /// <seealso cref="IServerChatCommandBroker" />
    public abstract class ServerChatCommandBroker : IServerChatCommandBroker
    {
        /// <summary>
        ///     A collection of command handlers, indexed by a string representation of the command name.
        /// </summary>
        public Dictionary<string, ServerChatCommandDelegate> Options { get; } = new();

        /// <summary>
        ///     Called when a Server Admin changes the settings for the GPS feature.
        /// </summary>
        /// <param name="player">The player that initiated the command.</param>
        /// <param name="groupId">The chat group to pass messages back to.</param>
        /// <param name="args">The arguments passed in, by the user.</param>
        public void OnServerConfigCommand(IServerPlayer player, int groupId, CmdArgs args)
        {
            var section = args.PopWord();
            if (!Options.ContainsKey(section))
            {
                player.SendInvalidSyntaxMessage(groupId);
                return;
            }
            Options[section](player, groupId, args);
        }
    }
}