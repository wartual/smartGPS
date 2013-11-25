using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Duration
    {
        [JsonProperty("text")]
        public String Text { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    
    }
}