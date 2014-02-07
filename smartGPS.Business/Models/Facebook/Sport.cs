using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class Sport
    {
        [JsonProperty("id")]
        public String Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        //[JsonProperty("with")]
        //public List<BasicInformation> With { get; set; }

        //[JsonProperty("from")]
        //public List<BasicInformation> From { get; set; }
    }
}