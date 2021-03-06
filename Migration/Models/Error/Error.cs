﻿using Newtonsoft.Json;

namespace Konference.Models
{
    class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("validation_errors")]
        public ValidationError[] ValidationErrors { get; set; }
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }

    class ValidationError
    {
        public string Message { get; set; }
    }
}
