using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class User
    {
        public static String SELF = "self";

        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("firstName")]
        public String FirstName { get; set; }

        [JsonProperty("lastName")]
        public String lastName { get; set; }

        [JsonProperty("gender")]
        public String Gender { get; set; }

        [JsonProperty("photo")]
        public Icon Photo { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("relationship")]
        public String Relationship { get; set; }

        public String FullName
        {
            get
            {
                return this.FirstName + " " + this.lastName;
            }
        }
    }
}