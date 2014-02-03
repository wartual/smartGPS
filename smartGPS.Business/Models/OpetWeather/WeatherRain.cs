using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.OpetWeather
{
    public class WeatherRain
    {
        [JsonProperty("3h")]
        public double Last3Hours { get; set; }
    }
}