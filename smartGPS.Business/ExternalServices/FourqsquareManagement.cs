using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using smartGPS.Business.Models.Foursquare;

namespace smartGPS.Business.ExternalServices
{
    public class FourqsquareManagement
    {

        public static FoursquareExploreVenueResponse getExploreVenues(double latitude, double longitude)
        {
            FoursquareExploreVenueResponse model = null;
            String url = APICalls.getFoursquareExploreVenuesUrl(latitude, longitude, Config.PLACES_RADIUS, null);

            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        model = JsonConvert.DeserializeObject<FoursquareExploreVenueResponse>(responseString);
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