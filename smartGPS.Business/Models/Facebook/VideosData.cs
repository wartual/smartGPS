using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class VideosData
    {
        
        [JsonProperty("data")]
        public Video Video;

        [JsonProperty("type")]
        public String Type;
    }
}