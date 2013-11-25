using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Geometry
    {
        [JsonProperty("location")]
        public Location Locations { get; set; }
    }
}