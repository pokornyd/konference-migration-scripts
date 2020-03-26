using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konference
{
    class Properties
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }
    }
}
