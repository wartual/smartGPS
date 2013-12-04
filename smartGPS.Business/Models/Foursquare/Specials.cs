using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Specials
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("Items")]
        public List<Items> Items { get; set; }
    }
}