using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Items
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("message")]
        public String Message { get; set; }

        [JsonProperty("description")]
        public String Description { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("page")]
        public Page Page { get; set; }
    }
}