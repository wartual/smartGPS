using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Groups
    {
        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("items")]
        public List<Items> Items { get; set; }
    }
}