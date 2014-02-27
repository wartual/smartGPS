using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APITravel
    {
        [JsonProperty("id")]
        public String id { get; set; }

        [JsonProperty("userId")]
        public String userId { get; set; }

        [JsonProperty("departureAddress")]
        public String departureAddress { get; set; }

        [JsonProperty("departureLatitude")]
        public double departureLatitude { get; set; }

        [JsonProperty("departureLongitude")]
        public double departureLongitude { get; set; }

        [JsonProperty("destinationAddress")]
        public String destinationAddress { get; set; }

        [JsonProperty("destinationLatitude")]
        public double destinationLatitude { get; set; }

        [JsonProperty("destinationLongitude")]
        public double destinationLongitude { get; set; }

        [JsonProperty("currentLatitude")]
        public double currentLatitude { get; set; }

        [JsonProperty("currentLongitude")]
        public double currentLongitude { get; set; }

        [JsonProperty("time")]
        public double time { get; set; }

        [JsonProperty("distance")]
        public double distance { get; set; }

        [JsonProperty("statusId")]
        public String statusId { get; set; }

        [JsonProperty("dateCreated")]
        public long dateCreated { get; set; }

        [JsonProperty("dateUpdated")]
        public long dateUpdated { get; set; }

        [JsonProperty("directions")]
        public String directions { get; set; }
    }
}