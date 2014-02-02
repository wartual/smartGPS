using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class Like
    {
        [JsonProperty("category")]
        public String Category {get; set;}

        [JsonProperty("name")]
        public String Name {get; set;}

        [JsonProperty("created_time")]
        public String TimeCreated {get; set;}

        [JsonProperty("id")]
        public String Id {get; set;}

        [JsonProperty("category_list")]
        public  List<CategoryList> ListCategories {get; set;}

    }
}