using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Recent
    {

        [JsonProperty("createdAt")]
        public String CreatedAt { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("venue")]
        public Venues Venue { get; set; }
    }
}