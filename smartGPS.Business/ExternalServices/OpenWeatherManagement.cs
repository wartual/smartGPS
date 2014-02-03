using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using smartGPS.Business.Models.OpetWeather;

namespace smartGPS.Business.ExternalServices
{
    public class OpenWeatherManagement
    {

        public static WeatherResponse getWeatherByGPS(double latitude, double longitude)
        {
            WeatherResponse model = null;
            String url = null;

            url = APICalls.getWeatherUrl(latitude, longitude);
          
            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<WeatherResponse>(responseString);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}