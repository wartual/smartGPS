using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class Paging
    {
        [JsonProperty("previous")]
        public String Previous { get; set; }

        [JsonProperty("Next")]
        public String Next { get; set; }
    }
}