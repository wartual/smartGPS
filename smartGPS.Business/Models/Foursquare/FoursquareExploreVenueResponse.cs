using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class FoursquareExploreVenueResponse
    {
        [JsonProperty("response")]
        public ExploreResponse Response { get; set; }

        [JsonProperty("numResults")]
        public String NumResults { get; set; }
    }
}