using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Models.UserAdministration
{
    public class APIDeactivateNotification
    {

        [JsonProperty("userId")]
        public String userId { get; set; }

        [JsonProperty("notificationId")]
        public String notificationId { get; set; }
    }
}