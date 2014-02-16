using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.AStar
{
    public class SmartNode
    {
        [JsonProperty("latitude")]
        public double latitude { get; set; }

        [JsonProperty("longitude")]
        public double longitude { get; set; }

        [JsonProperty("id")]
        public double id { get; set; }

        public SmartNode()
        {
            latitude = 0;
            longitude = 0;
            id = -1;
        }

        public SmartNode(double _lat, double _long, double _id)
        {
            latitude = _lat;
            longitude = _long;
            id = _id;
        }
    }
}