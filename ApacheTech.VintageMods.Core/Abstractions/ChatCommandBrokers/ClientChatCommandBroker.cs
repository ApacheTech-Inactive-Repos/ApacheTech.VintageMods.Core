using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.ChatCommandBrokers
{
    /// <summary>
    ///     Delegates the handling of client-side chat commands, mapped through an internal dictionary, indexed by the name of the command.
    /// </summary>
    /// <seealso cref="IClientChatCommandBroker" />
    public abstract class ClientChatCommandBroker : IClientChatCommandBroker
    {
        /// <summary>
        ///     A collection of command handlers, indexed by a string representation of the command name.
        /// </summary>
        public Dictionary<string, ClientChatCommandDelegate> Options { get; } = new();

        /// <summary>
        ///     Delegates the handling of a command, through the <see cref="Options"/> collection.
        /// </summary>
        /// <param name="player">The player that initiated the command.</param>
        /// <param name="groupId">The chat group to pass messages back to.</param>
        /// <param name="args">The arguments passed in, by the user.</param>
        public void OnClientConfigCommand(IPlayer player, int groupId, CmdArgs args)
        {
            var section = args.PopWord();
            if (!Options.ContainsKey(section))
            {
                player.SendInvalidSyntaxMessage(groupId);
                return;
            }
            Options[section](groupId, args);
        }
    }
}