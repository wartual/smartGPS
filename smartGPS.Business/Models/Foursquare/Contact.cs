using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Contact
    {
        [JsonProperty("phone")]
        public String Phone { get; set; }

        [JsonProperty("formattedPhone")]
        public String FormattedPhone { get; set; }

        [JsonProperty("twitter")]
        public String Twitter { get; set; }

    }
}