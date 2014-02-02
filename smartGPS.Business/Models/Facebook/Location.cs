using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class Location
    {
        [JsonProperty("street")]
        public String Street { get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        [JsonProperty("state")]
        public String State { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

        [JsonProperty("zip")]
        public String Zip { get; set; }

        [JsonProperty("latitude")]
        public long Latitude { get; set; }

        [JsonProperty("longitude")]
        public long Longitude { get; set; }
    }
}