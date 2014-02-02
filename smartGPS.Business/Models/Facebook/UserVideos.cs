using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserVideos
    {
        [JsonProperty("data")]
        public List<VideosData> VideosData { get; set; }
    }
}