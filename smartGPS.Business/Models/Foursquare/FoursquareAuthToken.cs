using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class FoursquareAuthToken
    {
        [JsonProperty("access_token")]
        public String AuthToken { get; set; }
    }
}