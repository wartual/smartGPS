using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.ExternalServices
{
    public class Config
    {
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

        public static int LIKES_BOOK_LIMIT = 1;

        public static int LIKES_SPORTS = 3;

        public static int LIKES_TRAVELING = 3;

        public static double CBA_DEFAULT_SUPPORT = 0.2;

        public static double CBA_DEFAULT_CONFIDENCE = 0.3;

        public static int KMEANS_DEFAULT_K = 3;

        public static int KNN_DEFAULT_K = 3;
    }
}