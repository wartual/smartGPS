using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class GoogleGeoCodeResponse
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("results")]
        public List<Results> Results { get; set; }
    }
}