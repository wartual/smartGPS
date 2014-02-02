using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserLikes
    {
        [JsonProperty("data")]
        public List<Like> Likes { get; set; }
    }
}