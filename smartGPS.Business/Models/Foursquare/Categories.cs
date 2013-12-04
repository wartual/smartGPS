using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Categories
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("pluralName")]
        public String PluralName { get; set; }

        [JsonProperty("shortName")]
        public String ShortName { get; set; }

        [JsonProperty("primary")]
        public Boolean Primary { get; set; }

        [JsonProperty("icon")]
        public Icon Icon { get; set; }

    }
}