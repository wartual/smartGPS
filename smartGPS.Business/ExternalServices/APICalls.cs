using System;
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
        public static String FOURSQUARE_EXPLORE_VENUES = "https://api.foursquare.com/v2/venues/explore?";
        public static String GOOGLE_PLACES_TEXT = "https://maps.googleapis.com/maps/api/place/textsearch/json?sensor=false&query=";
        public static String FOURSQUARE_REDIRECT_URL = "Dashboard/ExternalServices/FoursquareResponse";
        public static String FOURSQUARE_VENUES_HISTORY = "https://api.foursquare.com/v2/users/self/checkins?oauth_token=";
        public static String FOURSQUARE_RECENT = "https://api.foursquare.com/v2/checkins/recent?oauth_token=";
        public static String FOURSQUARE_AUTHENTICATE_URL = "https://foursquare.com/oauth2/authenticate?client_id=";
        public static String FOURSQUARE_AUTH_TOKEN_URL = "https://foursquare.com/oauth2/access_token?client_id=";
        public static String WEATHER_URL = "http://api.openweathermap.org/data/2.5/weather?";
        public static String ROAD_CONDITIONS_URL = "http://opendata.si/promet/events/";
        public static String FOURSQUARE_VENUES_CATEGORY = "https://api.foursquare.com/v2/venues/categories?oauth_token=";

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

        public static String getFoursquareExploreVenuesUrl(double latitude, double longitude, double radius, List<String> sections)
        {
            return FOURSQUARE_EXPLORE_VENUES + "client_id=" + Config.FOURSQUARE_CLIENT_ID + "&client_secret=" + Config.FOURSQUARE_CLIENT_SECRET +
                 "&v=20131201&" + "ll=" + latitude.ToString().Replace(",",".") + "," + longitude.ToString().Replace(",",".") +  "&radius=" + radius;
        }

        public static String getPlacesByTextFormattedUrl(String text, double radius)
        {
            return GOOGLE_PLACES_TEXT + text + "&radius=" + radius.ToString().Replace(",", ".") + "&key=" + Config.GOOGLE_SERVER_API;
        }

        public static String getFoursquareAuthTokenWithCode(String code)
        {
            return FOURSQUARE_AUTH_TOKEN_URL + Config.FOURSQUARE_CLIENT_ID + "&client_secret=" + Config.FOURSQUARE_CLIENT_SECRET + "&grant_type=authorization_code&redirect_uri=" + Enviroment.DEVELOPMENT_SERVER_URL + FOURSQUARE_REDIRECT_URL + "&code=" + code;
        }

        public static String getFoursquareAuthTokenUrl()
        {
            return FOURSQUARE_AUTHENTICATE_URL + Config.FOURSQUARE_CLIENT_ID + "&response_type=code&redirect_uri=" + Enviroment.DEVELOPMENT_SERVER_URL + FOURSQUARE_REDIRECT_URL;
        }

        public static String getFoursquareCheckins(String authToken)
        {
            return FOURSQUARE_VENUES_HISTORY + authToken + "&v=20131201";
        }

        public static String getFoursquareRecentCheckins(String authToken)
        {
            return FOURSQUARE_RECENT + authToken + "&v=20131201&afterTimestamp=1325444622";
        }

        public static String getWeatherUrl(double latitude, double longitude)
        {
            return WEATHER_URL +"lat=" + latitude + "&lon=" + longitude;
        }

        public static String getRoadConditionsUrl()
        {
            return ROAD_CONDITIONS_URL;
        }

        public static String getFoursquareVenuesCateogories(String authToken)
        {
            return FOURSQUARE_VENUES_CATEGORY + authToken + "&v=20131201";
        }
    }
}