using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Dashboard.Models
{
    public class TravelModel
    {

        public String Id { get; set; }

        public String DepartureAddress { get; set; }

        public Double DepartureLatitude { get; set; }

        public Double DepartureLongitude { get; set; }

        public String DestinationAddress { get; set; }

        public Double DestinationLatitude { get; set; }

        public Double DestinationLongitude { get; set; }

        public Double CurrentLatitude { get; set; }

        public Double CurrentLongitude { get; set; }

        public Double Time { get; set; }

        public Double Distance { get; set; }

        public String Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public String Directions { get; set; }

        public int mode { get; set; }
    }
}