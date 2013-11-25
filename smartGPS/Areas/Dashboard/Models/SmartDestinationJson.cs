using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Areas.Dashboard.Models
{
    public class SmartDestinationJson
    {
        [JsonProperty("address")]
        public String Address { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

         [JsonProperty("value")]
        public int Value { get; set; }
    }
}