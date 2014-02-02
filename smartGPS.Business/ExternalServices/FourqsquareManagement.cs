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

        public static Boolean obtainAuthToken(String code, String userId)
        {
            String url = APICalls.getFoursquareAuthTokenWithCode(code);
            FoursquareAuthToken token;
            WebRequest request = WebRequest.Create(url);

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        String responseString = new StreamReader(stream).ReadToEnd();
                        token = JsonConvert.DeserializeObject<FoursquareAuthToken>(responseString);
                        UserAdministration.updateFoursquareId(userId, token.AuthToken);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static FoursquareStatistics importVenusHistory(String userId)
        {
            smartGPS.Persistance.User user = UserAdministration.getUserByUserId(userId);

            if (user == null || user.FoursquareId == null)
                return null;
            else
            {
                String url = APICalls.getFoursquareCheckins(user.FoursquareId);
                FoursquareVenuesHistoryResponse checkins;
                FoursquareRecentCheckinsResponse recent;
                FoursquareStatistics statistics;
                FoursquareDataMining dataMining;
                WebRequest request = WebRequest.Create(url);
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            String responseString = new StreamReader(stream).ReadToEnd();
                            checkins = JsonConvert.DeserializeObject<FoursquareVenuesHistoryResponse>(responseString);
                        }
                    }

                    url = APICalls.getFoursquareRecentCheckins(user.FoursquareId);
                    request = WebRequest.Create(url);
         
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            String responseString = new StreamReader(stream).ReadToEnd();
                            recent = JsonConvert.DeserializeObject<FoursquareRecentCheckinsResponse>(responseString);
                        }
                    }
                    dataMining = new FoursquareDataMining(checkins, recent);
                    statistics = dataMining.analyze();
                    return statistics;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}