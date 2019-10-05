using Newtonsoft.Json;

namespace WFIWLib.Models
{
    class ReviewCreationObject : ReviewObject
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        public ReviewCreationObject(int movieId, int status)
          : base(movieId)
        {
            Status = status;
        }
    }
}
