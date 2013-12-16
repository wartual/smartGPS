using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APIProfileModel
    {
        [JsonProperty("userId")]
        public String UserId { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("dateofBirth")]
        public long? DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public Boolean? Gender { get; set; }

        [JsonProperty("address")]
        public String Address { get; set; }

        [JsonProperty("postalOffice")]
        public String PostalOffice { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

        [JsonProperty("phone")]
        public String Phone { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("surname")]
        public String Surname { get; set; }
    }
}