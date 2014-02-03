using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.OpetWeather
{
    public class WeatherDescription
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("main")]
        public String Main { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("icon")]
        public String Icon { get; set; }
    }
}