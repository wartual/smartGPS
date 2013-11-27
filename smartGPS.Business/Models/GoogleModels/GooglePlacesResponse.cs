using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class GooglePlacesResponse
    {

        [JsonProperty("next_page_token")]
        public String NextPageToken;

        [JsonProperty("results")]
        public List<PlacesResult> Results { get; set; }

        [JsonProperty("status")]
        public String Status { get; set; }

    }
}