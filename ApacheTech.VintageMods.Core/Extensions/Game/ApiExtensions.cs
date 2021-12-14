using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class ApiExtensions
    {
        #region Client

        public static ClientMain AsClientMain(this ICoreClientAPI api)
        {
            return (ClientMain)api.World;
        }

        public static ICoreClientAPI AsApi(this ClientMain game)
        {
            return (ICoreClientAPI)game.Api;
        }

        #endregion

        #region Server

        public static ServerMain AsServerMain(this ICoreServerAPI api)
        {
            return (ServerMain)api.World;
        }

        public static ICoreServerAPI AsApi(this ServerMain game)
        {
            return (ICoreServerAPI)game.Api;
        }

        #endregion

        #region ICoreAPI

        /// <summary>
        ///     Gets the world seed.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static string GetSeed(this ICoreAPI api)
        {
            return api.World?.Seed.ToString();
        }

        /// <summary>
        ///     Converts an agnostic API to a Server-side API.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static ICoreServerAPI ForServer(this ICoreAPI api)
        {
            if (api.Side.IsClient()) return null;
            return api as ICoreServerAPI;
        }

        /// <summary>
        ///     Converts an side-agnostic API to a client-side API.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static ICoreClientAPI ForClient(this ICoreAPI api)
        {
            if (api.Side.IsServer()) return null;
            return api as ICoreClientAPI;
        }

        #endregion
    }
}
