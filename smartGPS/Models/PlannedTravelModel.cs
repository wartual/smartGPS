using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models
{
    public class PlannedTravelModel
    {
        [JsonProperty("departureLatitude")]
        public double departureLatitude { get; set; }

        [JsonProperty("departureLongitude")]
        public double departureLongitude { get; set; }

        [JsonProperty("destinationLatitude")]
        public double destinationLatitude { get; set; }

        [JsonProperty("destinationLongitude")]
        public double destinationLongitude { get; set; }

        [JsonProperty("destinationAddress")]
        public String destinationAddress { get; set; }

        [JsonProperty("departureAddress")]
        public String departureAddress { get; set; }

        [JsonProperty("proposedTravel")]
        public bool proposedTravel { get; set; }


    }
}