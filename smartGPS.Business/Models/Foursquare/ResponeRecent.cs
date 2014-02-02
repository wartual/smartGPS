using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class ResponeRecent
    {
        [JsonProperty("recent")]
        public List<Recent> RecentCheckings { get; set; }
    }
}