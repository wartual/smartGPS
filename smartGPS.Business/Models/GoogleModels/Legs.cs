using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Legs
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("end_location")]
        public Location EndLocation { get; set; }

        [JsonProperty("start_location")]
        public Location StartLocation { get; set; }

        [JsonProperty("end_address")]
        public String EndAddress { get; set; }

        [JsonProperty("start_address")]
        public String StartAddress { get; set; }

        [JsonProperty("steps")]
        public Steps[] Steps { get; set; }

    }
}