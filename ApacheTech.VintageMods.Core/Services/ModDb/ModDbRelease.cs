using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApacheTech.VintageMods.Core.Services.ModDb
{
    [JsonObject]
    public class ModDbRelease
    {
        [JsonProperty("fileid")]
        public int FileId { get; set; }

        [JsonProperty("modidstr")]
        public string ModId { get; set; }

        [JsonProperty("mainfile")]
        public string FileRelativeUrl { get; set; }

        [JsonProperty("filename")]
        public string FileName { get; set; }

        [JsonProperty("downloads")]
        public string DownloadCount { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("modversion")]
        public string Version { get; set; }

        [JsonProperty("created")]
        public DateTime CreatedAt { get; set; }
    }
}