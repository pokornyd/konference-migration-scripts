using Newtonsoft.Json;

namespace Konference.Models
{
    class LanguageVariants
    {
        [JsonProperty("variants")]
        public Variant[] Variants { get; set; }
    }

    public class Variant
    {
        [JsonProperty("elements")]
        public VariantElements[] VariantElements { get; set; }
        [JsonProperty("item")]
        public VariantItem Item { get; set; }


    }

    public class VariantElements
    {
        [JsonProperty("element")]
        public VariantElement Element { get; set; }
        [JsonProperty("value")]
        public dynamic Value { get; set; }
        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        public string Mode { get; set; }
        [JsonProperty("components")]
        public Component[] Components { get; set; }
    }

    public class VariantElement
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }

    public class VariantItem
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }

    public class Component
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("type")]
        public Type Type { get; set; }
        [JsonProperty("elements")]
        public VariantElements[] VariantElements { get; set; }
    }
}
