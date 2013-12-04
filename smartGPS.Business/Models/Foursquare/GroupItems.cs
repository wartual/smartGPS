using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class GroupItems
    {
        [JsonProperty("venue")]
        public Venues Venue { get; set; }

        [JsonProperty("tips")]
        public List<Tips> Tips { get; set; }

        [JsonProperty("referralId")]
        public String ReferralId { get; set; }
    }
}