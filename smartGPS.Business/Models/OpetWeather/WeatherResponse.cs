using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.OpetWeather
{
    public class WeatherResponse
    {
        [JsonProperty("coord")]
        public WeatherCoord Coordinates{get; set;}

        [JsonProperty("sys")]
        public WeatherSys Sys{get; set;}
        
        [JsonProperty("weather")]
        public List<WeatherDescription> Description{get; set;}

        [JsonProperty("main")]
        public WeatherMain Main{get; set;}
        
        [JsonProperty("wind")]
        public WeatherWind Wind{get; set;}
        
        [JsonProperty("rain")]
        public WeatherRain Rain{get; set;}
        
        [JsonProperty("name")]
        public String Name{get; set;}
    }
}