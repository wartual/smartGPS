using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserCheckins
    {
        [JsonProperty("data")]
        public List<CheckinData> Checkins { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }
}