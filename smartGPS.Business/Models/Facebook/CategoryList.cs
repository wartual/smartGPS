using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class CategoryList
    {
        [JsonProperty("id")]
        public String Id {get; set;}

        [JsonProperty("name")]
        public String Name {get; set;}
    }
}