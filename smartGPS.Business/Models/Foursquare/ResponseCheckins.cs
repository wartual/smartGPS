using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class ResponseCheckins
    {
        [JsonProperty("checkins")]
        public Checkins Checkins { get; set; }
    }
}