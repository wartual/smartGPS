﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Page
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("firstName")]
        public String FirstName { get; set; }

        [JsonProperty("gender")]
        public String Gender { get; set; }

        [JsonProperty("photo")]
        public Icon Photo { get; set; }

        [JsonProperty("homecity")]
        public String HomeCity { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }
    }
}