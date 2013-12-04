using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class ExploreResponse
    {
        [JsonProperty("suggestedFilters")]
        public SuggestedFilters SuggestedFilters { get; set; }

        [JsonProperty("suggestedRadius")]
        public long SuggestedRadius { get; set; }

        [JsonProperty("headerLocation")]
        public String HeaderLocation { get; set; }

        [JsonProperty("headerFullLocation")]
        public String HeaderFullLocation { get; set; }

        [JsonProperty("headerLocationGranularity")]
        public String HeaderLocationGranularity { get; set; }

        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("groups")]
        public List<Groups> Groups { get; set; }
    }
}