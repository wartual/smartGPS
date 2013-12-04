using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Icon
    {
        [JsonProperty("prefix")]
        public String Prefix { get; set; }

        [JsonProperty("sufx")]
        public String Sufix { get; set; }
    }
}