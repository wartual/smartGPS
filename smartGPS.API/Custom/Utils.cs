using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace smartGPS.API.Custom
{
    public class Utils
    {
        public static JsonMediaTypeFormatter getJsonNetForrmater()
        {
            JsonMediaTypeFormatter formatter = new JsonMediaTypeFormatter();

            formatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            formatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            formatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            formatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatter.SerializerSettings.Culture = new CultureInfo("en-US");

            return formatter;
        }
    }
}