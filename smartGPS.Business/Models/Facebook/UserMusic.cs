using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserMusic
    {
        [JsonProperty("data")]
        public List<Music> Music { get; set; }
    }
}