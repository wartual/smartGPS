using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APIAddNotification
    {
        [JsonProperty("userId")]
        public String userId { get; set; }

        [JsonProperty("latitude")]
        public double latitude { get; set; }

        [JsonProperty("longitude")]
        public double longitude { get; set; }

        [JsonProperty("text")]
        public String text { get; set; }

        [JsonProperty("category")]
        public String category { get; set; }

        [JsonProperty("dateCreated")]
        public long dateCreated { get; set; }

        [JsonProperty("username")]
        public String username { get; set; }

        [JsonProperty("address")]
        public String address { get; set; }
    }
}