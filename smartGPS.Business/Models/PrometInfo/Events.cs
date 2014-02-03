using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.PrometInfo
{
    public class Events
    {
        [JsonProperty("dogodek")]
        public List<Event> Event { get; set; }
    }
}