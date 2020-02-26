using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Konference.Models
{
    public class ContentItems
    {
        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("codename")]
        public string Codename { get; set; }
        [JsonProperty("type")]
        public Type Type { get; set; }
    }

    public class Type
    {
        [JsonProperty("codename")]
        public string Codename { get; set; }
    }
}
