﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.ExternalServices
{
    public class APICalls
    {
        public static String GOOGLE_MAP_APIS = "http://maps.googleapis.com/maps/api/geocode/json?sensor=false&";
        public static String GOOGLE_MAP_DIRECTIONS = "http://maps.googleapis.com/maps/api/directions/json?origin=";
        public static String GOOGLE_PLACES = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?sensor=false&location=";

        public static String getMapDirectionsFormattedUrl(double startLatitude, double startLongitude, double endLatitude, double endLongitude, String mode)
        {
            return GOOGLE_MAP_DIRECTIONS + startLatitude.ToString().Replace(",", ".") + "," + startLongitude.ToString().Replace(",", ".") + "&destination=" + endLatitude.ToString().Replace(",", ".")
                + "," + endLongitude.ToString().Replace(",", ".") + "&sensor=false&units=metric&mode=" + mode; 
        }

        public static String getPlacesFormattedUrl(double startLatitude, double startLongitude, double radius)
        {
            return GOOGLE_PLACES + startLatitude.ToString().Replace(",", ".") + "," + startLongitude.ToString().Replace(",", ".")
                + "&radius=" + radius.ToString().Replace(",", ".") + "&key=" +  Config.GOOGLE_SERVER_API;
        }
    }
}