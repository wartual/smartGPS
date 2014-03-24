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

        [JsonProperty("type")]
        public String type;

        [JsonProperty("iconPrefix")]
        public String prefix;

        [JsonProperty("iconSufix")]
        public String sufix;

        [JsonProperty("title")]
        public String title;

        [JsonProperty("category")]
        public String category;

        public SmartNode()
        {
            latitude = 0;
            longitude = 0;
            id = -1;
            type = "regular";
            prefix = null;
            sufix = null;
            title = null;
            category = null;
        }

        public SmartNode(double _lat, double _long, double _id)
        {
            latitude = _lat;
            longitude = _long;
            id = _id;
            type = "regular";
            prefix = null;
            sufix = null;
            title = null;
            category = null;
        }

        public SmartNode(double _lat, double _long, string _type)
        {
            latitude = _lat;
            longitude = _long;
            id = -1;
            type = _type;
            prefix = null;
            sufix = null;
            title = null;
            category = null;
        }

        public SmartNode(double _lat, double _long, double _id,  string _type, String _prefix, String _sufix, String _title, String _category)
        {
            latitude = _lat;
            longitude = _long;
            id = _id;
            type = _type;
            prefix = _prefix;
            sufix = _sufix;
            title = _title;
            category = _category;
        }
    }
}