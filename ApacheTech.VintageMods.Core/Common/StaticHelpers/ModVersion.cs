using System;
using ApacheTech.VintageMods.Core.Services;

namespace ApacheTech.VintageMods.Core.Common.StaticHelpers
{
    public static class ModVersion
    {
        public static Version ArchiveVersion()
        {
            return Version.Parse(ApiEx.ModInfo.Version);
        }

        public static Version InstalledVersion(string fileName)
        {
            return Version.Parse(
                ModServices.FileSystem
                    .GetJsonFile(fileName)
                    .ParseAsJsonObject()
                    ["Version"].AsString("1.0.0.0"));
        }
    }
}