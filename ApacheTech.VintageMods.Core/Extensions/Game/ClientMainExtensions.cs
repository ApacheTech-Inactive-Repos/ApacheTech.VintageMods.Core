using System.Collections.Generic;
using ApacheTech.Common.Extensions.Harmony;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    public static class ClientMainExtensions
    {
        public static void StopAllSounds(this ClientMain game)
        {
            var activeSounds = game.GetField<Queue<ILoadedSound>>("ActiveSounds");
            foreach (var sound in activeSounds)
            {
                sound.Stop();
            }
        }
    }
}