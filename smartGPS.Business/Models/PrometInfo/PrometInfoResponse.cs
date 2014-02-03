using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.PrometInfo
{
    public class PrometInfoResponse
    {
        [JsonProperty("dogodki")]
        public Events Events { get; set; }

        [JsonProperty("updated")]
        public double Updated { get; set; }

        [JsonProperty("copyright")]
        public String Copyright { get; set; }
    }
}