using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class FoursquareVenuesCategoriesResponse
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("response")]
        public VenueCategories Categories { get; set; }
    }
}