using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Location
    {
        [JsonProperty("address")]
        public String Address { get; set; }

        [JsonProperty("crossStreet")]
        public String CrossStreet { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("postalCode")]
        public String PostalCode { get; set; }

        [JsonProperty("cc")]
        public String CountryCode { get; set; }

        [JsonProperty("city")]
        public String City { get; set; }

        [JsonProperty("state")]
        public String State { get; set; }

        [JsonProperty("country")]
        public String Country { get; set; }

    }
}