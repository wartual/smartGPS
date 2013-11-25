using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Steps
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("end_location")]
        public Location EndLocation { get; set; }

        [JsonProperty("start_location")]
        public Location StartLocation { get; set; }

        [JsonProperty("travel_mode")]
        public String TravelMode { get; set; }

        [JsonProperty("html_instructions")]
        public String Instructions { get; set; }

        [JsonProperty("polyline")]
        public Polyline Polyline { get; set; }

        [JsonProperty("maneuver")]
        public String Maneuver { get; set; }
    }
}