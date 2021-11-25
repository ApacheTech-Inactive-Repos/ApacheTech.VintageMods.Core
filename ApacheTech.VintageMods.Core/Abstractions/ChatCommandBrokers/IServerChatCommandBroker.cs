using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Abstractions.ChatCommandBrokers
{
    /// <summary>
    ///     Delegates the handling of server-side chat commands, mapped through an internal dictionary, indexed by the name of the command.
    /// </summary>
    public interface IServerChatCommandBroker
    {
        /// <summary>
        ///     A collection of command handlers, indexed by a string representation of the command name.
        /// </summary>
        public Dictionary<string, ServerChatCommandDelegate> Options { get; }

        /// <summary>
        ///     Delegates the handling of a command, through the <see cref="Options"/> collection.
        /// </summary>
        /// <param name="player">The player that initiated the command.</param>
        /// <param name="groupId">The chat group to pass messages back to.</param>
        /// <param name="args">The arguments passed in, by the user.</param>
        public void OnServerConfigCommand(IServerPlayer player, int groupId, CmdArgs args);
    }
}