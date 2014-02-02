using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserCheckins
    {
        [JsonProperty("data)")]
        public List<Place> Checkins { get; set; }
    }
}