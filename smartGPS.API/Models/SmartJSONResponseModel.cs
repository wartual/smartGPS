using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.API.Models
{
    public class SmartJSONResponseModel
    {

        [JsonProperty("status")]
        public String Status { get; set; }

        [JsonProperty("message")]
        public String Message { get; set; }

    }
}