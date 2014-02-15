using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class VenueCategories
    {
        [JsonProperty("categories")]
        public List<Categories> Categories { get; set; }
    }
}