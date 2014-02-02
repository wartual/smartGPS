using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class BookData
    {
        [JsonProperty("data")]
        public Book Book;

        [JsonProperty("type")]
        public String Type;
    }
}