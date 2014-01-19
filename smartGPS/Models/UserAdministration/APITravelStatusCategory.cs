using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APITravelStatusCategory
    {
        [JsonProperty("id")]
        public String id { get; set; }

        [JsonProperty("type")]
        public String type { get; set; }
    }
}