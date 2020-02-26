using Newtonsoft.Json;

namespace Konference.Models
{
    class Folders
    {
        [JsonProperty("folders")]
        public Folder[] AssetFolders { get; set; }
    }

    class Folder
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
        [JsonProperty("folders")]
        public Folder[] Folders { get; set; }
    }
}
