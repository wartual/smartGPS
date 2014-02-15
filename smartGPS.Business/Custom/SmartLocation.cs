using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.Custom
{
    public class SmartLocation
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public SmartLocation(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public SmartLocation()
        {
        }
    }
}