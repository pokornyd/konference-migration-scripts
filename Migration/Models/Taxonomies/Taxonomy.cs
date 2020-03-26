using Newtonsoft.Json;

namespace Konference.Models
{
    class Taxonomy
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
        [JsonProperty("Codename")]
        public string Codename { get; set; }
        [JsonProperty("Terms")]
        public Term[] Terms { get; set; }
    }

    class Term
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("external_id")]
        public string External_id { get; set; }
        [JsonProperty("codename")]
        public string Codename { get; set; }
        [JsonProperty("terms")]
        public Term[] Terms { get; set; }
    }
}
