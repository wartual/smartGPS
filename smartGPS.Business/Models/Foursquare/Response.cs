using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Response
    {
        [JsonProperty("venues")]
        public List<Venues> Venues { get; set; }
    }
}