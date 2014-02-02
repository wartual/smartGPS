using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserFriends
    {
        [JsonProperty("data")]
        public List<FacebookProfileModel> Friends { get; set; }
   }
}