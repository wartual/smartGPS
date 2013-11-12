using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models
{
    public class FacebookProfileModel
    {
        [JsonProperty("first_name")]
        public String Name { get; set; }

        [JsonProperty("last_name")]
        public String Surname { get; set; }

        [JsonProperty("middle_name")]
        public String MiddleName { get; set; }

        [JsonProperty("gender")]
        public String Gender { get; set; }

        [JsonProperty("link")]
        public String Link { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("bio")]
        public String Biography { get; set; }

        [JsonProperty("birthday")]
        public String Birthday { get; set; }
    }
}