using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APIExternalLoginModel
    {
        [JsonProperty("username")]
        public String username { get; set; }

        [JsonProperty("password")]
        public String password { get; set; }
    }
}