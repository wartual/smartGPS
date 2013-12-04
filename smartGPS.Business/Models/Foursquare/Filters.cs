using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Filters
    {
        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("key")]
        public String Key { get; set; }
    }
}