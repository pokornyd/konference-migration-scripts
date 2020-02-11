using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Konference.Models
{
    class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("validation_errors")]
        public ValidationError[] ValidationErrors { get; set; }
    }

    class ValidationError
    {
        public string Message { get; set; }
    }
}
