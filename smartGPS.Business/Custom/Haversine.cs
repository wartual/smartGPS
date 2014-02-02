using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.Custom
{
    public class Haversine
    {
        public enum DistanceType { Miles, Kilometers };

        public double Distance(SmartLocation pos1, SmartLocation pos2, DistanceType type)
        {
            double preDlat = pos2.Latitude - pos1.Latitude;
            double preDlon = pos2.Longitude - pos1.Longitude;
            double R = (type == DistanceType.Miles) ? 3960 : 6371;
            double dLat = this.toRadian(preDlat);
            double dLon = this.toRadian(preDlon);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.Latitude)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;
            return d;
        }

        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}