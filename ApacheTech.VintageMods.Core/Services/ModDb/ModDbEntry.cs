using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApacheTech.VintageMods.Core.Services.ModDb
{
    [JsonObject]
    public class ModDbEntry
    {
        [JsonProperty("modid")]
        public int Id { get; set; }

        [JsonProperty("assetid")]
        public int AssetId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("text")]
        public string DescriptionHtml { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("logofilename")]
        public string LogoRelativeUrl { get; set; }

        [JsonProperty("homepageurl")]
        public string HomePageUrl { get; set; }

        [JsonProperty("sourcecodeurl")]
        public string SourceCodeUrl { get; set; }

        [JsonProperty("trailervideourl")]
        public string TrailerVideoUrl { get; set; }

        [JsonProperty("issuetrackerurl")]
        public string IssueTrackerUrl { get; set; }

        [JsonProperty("wikiurl")]
        public string WikiUrl { get; set; }

        [JsonProperty("downloads")]
        public int DownloadCount { get; set; }

        [JsonProperty("follows")]
        public int FollowCount { get; set; }

        [JsonProperty("comments")]
        public int CommentCount { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("created")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("lastmodified")]
        public DateTime ModifiedAt { get; set; }

        [JsonProperty("releases")]
        public List<ModDbRelease> Releases { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
    }
}