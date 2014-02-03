using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.OpetWeather
{
    public class WeatherSys
    {
        [JsonProperty("message")]
        public double Message { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

        [JsonProperty("sunrise")]
        public double Sunrise { get; set; }

        [JsonProperty("sunset")]
        public double Sunset { get; set; }
    }
}