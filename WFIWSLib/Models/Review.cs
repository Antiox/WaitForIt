using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFIWLib.Models
{
    public class Review
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("user_id")]
        public int UserID { get; set; }

        [JsonProperty("movie_id")]
        public int MovieID { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
