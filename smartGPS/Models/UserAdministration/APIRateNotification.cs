using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Models.UserAdministration
{
    public class APIRateNotification
    {

        public String userId { get; set; }

        public String notificationId { get; set; }

        public Boolean thumbsUp { get; set; }

        public Boolean thumbsDown { get; set; }
    }
}