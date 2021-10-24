using JetBrains.Annotations;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Common.Extensions
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ApiExtensions
    {
        #region Game Mode
        
        public static bool IsSurvival(this EnumGameMode mode) => mode == EnumGameMode.Survival;
        public static bool IsCreative(this EnumGameMode mode) => mode == EnumGameMode.Creative;
        public static bool IsSpectator(this EnumGameMode mode) => mode == EnumGameMode.Spectator;
        public static bool IsGuest(this EnumGameMode mode) => mode == EnumGameMode.Guest;

        #endregion

        #region ICoreAPI

        /// <summary>
        ///     Gets the world seed.
        /// </summary>
        /// <param name="api">The core game API.</param>
        [CanBeNull]
        public static string GetSeed(this ICoreAPI api)
        {
            return api?.World?.Seed.ToString();
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
