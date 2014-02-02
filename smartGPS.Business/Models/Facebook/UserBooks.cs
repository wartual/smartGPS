using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class UserBooks
    {
        [JsonProperty("data")]
        public List<BookData> BookData { get; set; }
    }
}