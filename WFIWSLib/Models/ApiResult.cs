using Newtonsoft.Json;
using System.Collections.Generic;

namespace WFIWLib.Models
{
    public class ApiResult
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("results")]
        public List<Review> Reviews { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        public bool HasError
        {
            get
            {
                return Error != null;
            }
        }
    }
}
