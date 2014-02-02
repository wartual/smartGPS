using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class CheckinItems
    {

        [JsonProperty("venue")]
        public Venues Venue { get; set; }

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("createdAt")]
        public String CreatedAt { get; set; }

        [JsonProperty("shout")]
        public String Comment { get; set; }
    }
}