using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class SeriesDeatails
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }
    }
}