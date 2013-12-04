using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Venues
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("categories")]
        public Categories Categories { get; set; }

        [JsonProperty("verified")]
        public Boolean Verified { get; set; }

        [JsonProperty("referralId")]
        public String ReferralId { get; set; }

        [JsonProperty("url")]
        public String Url { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("rating")]
        public double? Rating { get; set; }

        [JsonProperty("hours")]
        public Hours Hours { get; set; }

        [JsonProperty("specials")]
        public Specials Specials { get; set; }

    }
}