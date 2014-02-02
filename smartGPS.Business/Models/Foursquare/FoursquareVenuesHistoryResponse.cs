using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class FoursquareVenuesHistoryResponse
    {

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("response")]
        public ResponseCheckins Response { get; set; }
    }
}