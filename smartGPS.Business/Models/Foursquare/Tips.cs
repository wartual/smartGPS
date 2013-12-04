using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Tips
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("canonicalUrl")]
        public String CanonicalUrl { get; set; }

        [JsonProperty("text")]
        public String Text { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

    }
}