using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business
{
    public class Enviroment
    {
        public static String DEVELOPMENT
        {
            get
            {
                return "development";
            }
        }
        
        public static String PRODUCTION
        {
            get
            {
                return "production";
            }
        }

        public static String SERVER_URL{get; set;}
        public static String DEVELOPMENT_SERVER_URL = "http://localhost:5188/";
        public static String PRODUCTION_SERVER_URL = "http://smartgps.somee.com/";
    
        public static Boolean isProduction { get; set; }
    
        public static void setEnviroment(String environment)
        {
            if(environment.Equals(DEVELOPMENT))
            {
                isProduction = false;
                SERVER_URL = DEVELOPMENT_SERVER_URL;
            }
            else
            {
                isProduction = true;
                SERVER_URL = PRODUCTION_SERVER_URL;
            }
        }
    }
}