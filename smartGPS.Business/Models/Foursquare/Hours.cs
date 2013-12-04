using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Foursquare
{
    public class Hours
    {
        [JsonProperty("isOpen")]
        public Boolean IsOpen { get; set; }
    }
}