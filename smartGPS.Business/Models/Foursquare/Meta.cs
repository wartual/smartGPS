using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Meta
    {
        [JsonProperty("meta")]
        public int Code { get; set; }
    }
}