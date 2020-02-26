using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Konference.Models
{
    class Asset
    {
        [JsonProperty("file_reference")]
        public FileReference FileReference { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("descriptions")]
        public Description[] Descriptions { get; set; }
        [JsonProperty("folder")]
        public Folder Folder { get; set; }
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
    }

    class Description
    {
        [JsonProperty("language")]
        public Language Language { get; set; }
        [JsonProperty("description")]
        public string LocalizedDescription { get; set; }

    }

    class Language
    {
        [JsonProperty("codename")]
        string Codename { get; set; }
    }

    class FileReference
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
