using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Newtonsoft.Json.Linq;
using ProperVersion;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.Core.Services.ModDb
{
    public class ModDbContext : IDisposable
    {
        private readonly WebClient _client;

        public ModDbContext()
        {
            _client = new WebClient { BaseAddress = "https://mods.vintagestory.at/" };
        }


        public async Task<ModDbEntry> GetEntry()
        {
            var result = await _client.DownloadStringTaskAsync("https://mods.vintagestory.at/api/mod/46");
            var json = JObject.Parse(result);
            if (!json.TryGetValue("statuscode", out var code) || code.Value<int>() == 200)
            {
                return null;
            }
            return json["mod"].Value<ModDbEntry>();
        }

        private async Task<ModDbRelease> GetLatestRelease()
        {
            var entry = await GetEntry();
            return entry?.Releases[0];
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            var latestRelease = await GetLatestRelease();
            if (latestRelease is null) return false;
            var latestVersion = SemVer.Parse(latestRelease.Version);
            var currentVersion = SemVer.Parse(ApiEx.ModInfo.Version);
            return latestVersion > currentVersion;
        }

        public async Task DownloadRelease(ModDbRelease release)
        {
            var latestRelease = await GetLatestRelease();
            if (latestRelease is null) return;
            var fileBytes = await _client.DownloadDataTaskAsync($"https://mods.vintagestory.at/download?fileid={release.FileId}");
            var localPath = Path.Combine(GamePaths.DataPathMods, release.FileName);
            using var file = File.Create(localPath, fileBytes.Length, FileOptions.Asynchronous | FileOptions.SequentialScan);
            await file.WriteAsync(fileBytes, 0, fileBytes.Length);
            await file.FlushAsync();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}