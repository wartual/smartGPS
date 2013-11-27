using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class PlacesResult
    {
        [JsonProperty("icon")]
        public String IconUrl { get; set; }


        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }


        [JsonProperty("id")]
        public String Id { get; set; }


        [JsonProperty("name")]
        public String Name { get; set; }


        [JsonProperty("reference")]
        public String Reference { get; set; }

        [JsonProperty("vicinity")]
        public String Vicinity { get; set; }

        [JsonProperty("types")]
        public IList<string> Types { get; set; }
    }
}