using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.GoogleModels
{
    public class Results
    {
        [JsonProperty("formatted_address")]
        public String FormattedAddress;

        [JsonProperty("address_components")]
        public AddressComponents[] AddressComponents;

        [JsonProperty("geometry")]
        public Geometry Geometry;

        [JsonProperty("types")]
        public IList<string> Types { get; set; }
        
    }
}