using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class GoogleMapsDirectionsResponse
    {
        [JsonProperty("routes")]
        public Routes[] Routes { get; set; }

        [JsonProperty("status")]
        public String Status { get; set; }

    }
}