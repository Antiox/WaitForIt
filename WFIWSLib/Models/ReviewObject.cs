using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFIWLib.Models
{
    class ReviewObject
    {
        [JsonProperty("movie_id")]
        public int MovieId { get; set; }

        public ReviewObject(int movieId)
        {
            MovieId = movieId;
        }
    }
}
