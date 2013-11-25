using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class OverviewPolyline
    {
        [JsonProperty("points")]
        public String Points { get; set; }
    }
}