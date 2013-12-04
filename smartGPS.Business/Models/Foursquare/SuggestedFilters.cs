using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class SuggestedFilters
    {
        [JsonProperty("header")]
        public String Header { get; set; }

        [JsonProperty("id")]
        public List<Filters> Filters { get; set; }
    }
}