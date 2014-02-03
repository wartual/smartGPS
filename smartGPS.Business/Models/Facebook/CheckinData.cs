using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class CheckinData
    {
        [JsonProperty("place")]
        public Place Checkins { get; set; }

        [JsonProperty("id")]
        public String Id{get;set;}
    }
}