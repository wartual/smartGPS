using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Routes
    {
        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("legs")]
        public Legs[] Legs { get; set; }

        [JsonProperty("overview_polyline")]
        public OverviewPolyline OverviewPolyline { get; set; }

    }
}