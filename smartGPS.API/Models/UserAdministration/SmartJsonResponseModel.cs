using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.API.Models.UserAdministration
{
    public class SmartJsonResponseModel
    {
        [JsonProperty("status")]
        public String Status{get; set;}

        [JsonProperty("message")]
        public String Message{get; set;}
    }
}