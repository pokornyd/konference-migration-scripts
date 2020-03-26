using Newtonsoft.Json;

namespace Konference.Models
{
    class ContentTypes
    {
        [JsonProperty("types")]
        public ContentType[] Types { get; set; }
    }
    class ContentType
    {
        [JsonProperty("external_id")]
        public string External_id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("codename")]
        public string Codename { get; set; }
        [JsonProperty("content_groups")]
        public ContentGroup[] ContentGroups { get; set; }
        [JsonProperty("elements")]
        public ContentTypeElements[] Elements { get; set; }
    }

    class ContentTypeElements
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("codename")]
        public string Codename { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("guidelines")]
        public string Guidelines { get; set; }
        [JsonProperty("is_required")]
        public bool Is_required { get; set; }
        [JsonProperty("depends_on")]
        public DependingOnField Depends_on { get; set; }
        [JsonProperty("taxonomy_group")]
        public TaxonomyGroup Taxonomy_group { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("options")]
        public Options[] Options { get; set; }
        [JsonProperty("external_id")]
        public string ExternalId { get; set; }
        [JsonProperty("allowed_content_types")]
        public AllowedTypes[] AllowedTypes { get; set; }
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }
        [JsonProperty("content_group")]
        public ContentGroup ContentGroup { get; set; }
    }

    class DependingOnField
    {
        [JsonProperty("element")]
        public Element Element { get; set; }
    }

    class Element
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }

    class TaxonomyGroup
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
        
    }

    class ContentGroup
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("codename", NullValueHandling=NullValueHandling.Ignore)]
        public string Codename { get; set; }
        [JsonProperty("external_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalId { get; set; }
    }

    class Options
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    class AllowedTypes
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }

}
