using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Models.OpetWeather;
using smartGPS.Business.Models.PrometInfo;

namespace smartGPS.Business.ExternalServices
{
    public class Config
    {
        public static String TRAVEL_ACTIVE = "345a63cb-c017-479e-ba72-a1e34ae2ceaf";

        public static String getFacebookAppToken = "526242434133615|tlfuadfwXk4Qu5kS9L7W-DkDQFQ";

        public static String GOOGLE_SERVER_API = "AIzaSyAZM05G9vmxsBScFmW42iI0geoGmdryG0g";

        public static String GOOGLE_ANDROID_API = "AIzaSyCEeBcJW12qngWZxfhF-Gg91NkHvX-jFm8";

        public static String APP_ID = "680887826369";

        public static String FOURSQUARE_CLIENT_ID = "V22JFGZJFGU0N1EI2XPLWTCZMOMY5KPCMFZ0DUK05JPI0EVS";

        public static String FOURSQUARE_CLIENT_SECRET = "H42JQPLZAGLVLK0T4THIIH5CTYN2JLUWMSNOMLU4KRILE10M";

        public static double PLACES_RADIUS = 20000;

        public static double NOTIFICATION_RADIUS = 20;

        public static int NOTIFICATION_CONSENSUS = 20;

        public static int SORTED_LIMIT = 10;

        public static int LIKES_MUSIC_LIMIT = 8;

        public static int LIKES_MOVIES_LIMIT = 3;

        public static int LIKES_BOOK_LIMIT = 3;

        public static int LIKES_SPORTS = 3;

        public static int LIKES_TRAVELING = 3;

        public static double CBA_DEFAULT_SUPPORT = 0.2;

        public static double CBA_DEFAULT_CONFIDENCE = 0.3;

        public static int KMEANS_DEFAULT_K = 3;

        public static int KNN_DEFAULT_K = 3;

        public static int DECISION_TREES_CLASS_COUNT = 2;

        public static int FB_PROCCESSED_ENTRIES_ATTRIBUTE_COLUMNS = 6;

        public static int CONFUSION_MATRIX_NEGATIVE_CLASS_VALUE = -1;

        public static int RAIN_COEFFICINET = 15;

        public static int TRAFFIC_COEFFICINET = 20;

        public static double TRAFFIC_RADIUS = 0.2;

        public static List<int> WALKING_OPTIONS = new List<int>() { 5, 6 , 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static List<int> DRIVING_OPTIONS = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14, 15, 16 };

        public static int WAY_TYPE_FORBIDDEN_COEFFICIENT = 100;

        public static int getWeatherCoefficient(WeatherResponse weather)
        {
            int coeff = 0;

            // check weather
            foreach(WeatherDescription description in weather.Description)
            {
                if (description.Id > 201 && description.Id < 500)
                    // thunderstorm with rain
                    coeff = coeff + 50;
                else if (description.Id == 500)
                    // light rain
                    coeff = coeff + 10;
                else if (description.Id == 501)
                    // moderate rain
                    coeff = coeff + 20;
                else if (description.Id > 502 && description.Id < 600)
                    // heavz rain
                    coeff = coeff + 40;
                else if (description.Id == 600)
                    // light snow
                    coeff = coeff + 10;
                else if (description.Id == 601)
                    // snow
                    coeff = coeff + 20;
                else if (description.Id > 602 && description.Id < 700)
                    // heavy snow
                    coeff = coeff + 40;
                else if (description.Id >= 900)
                    //extreme weather
                    coeff = coeff + 1000;
            }

            return coeff;
        }

        public static int getRoadEventCoefficient(Event roadEvent)
        {
            if (roadEvent != null)
                return 100;
            else
                return 0;
        }

    }
}