using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using smartGPS.Business.Models.Foursquare;
using smartGPS.Persistance;

namespace smartGPS.Business.ExternalServices
{
    public class FourqsquareManagement
    {

        public static FoursquareExploreVenueResponse getExploreVenues(double latitude, double longitude)
        {
            FoursquareExploreVenueResponse model = null;
            String url = APICalls.getFoursquareExploreVenuesUrl(latitude, longitude, Config.PLACES_RADIUS);

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

        public static FoursquareExploreVenueResponse getExploreVenuesByCategories(double latitude, double longitude, String userId, int radius)
        {
            Profile profile = UserAdministration.getProfileByUserId(userId);

            if (profile == null || profile.Category == null)
            {
                return null;
            }
            else
            {
                FoursquareExploreVenueResponse model = null;
                String url = APICalls.getFoursquareExploreVenuesUrlByCategory(latitude, longitude, radius, profile.Category.Value);

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

        public static void getExploreCategories(String userId)
        {
            smartGPS.Persistance.User user = UserAdministration.getUserByUserId(userId);

            if (user == null || user.FoursquareId == null)
                return;
            else if(FoursquareDao.Categories_getAll().Count() != 0)
            {
                return;
            }
            else
            {
                String url = APICalls.getFoursquareVenuesCateogories(user.FoursquareId);
                FoursquareVenuesCategoriesResponse model;
                WebRequest request = WebRequest.Create(url);

                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            String responseString = new StreamReader(stream).ReadToEnd();
                            model = JsonConvert.DeserializeObject<FoursquareVenuesCategoriesResponse>(responseString);
                            persistVenueCategories(model);
                        }
                    }
                }
                catch (Exception e)
                {
                    return;
                }
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
                        getExploreCategories(userId);
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
                String responseHistory;
                String responseRecent;
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            responseHistory = new StreamReader(stream).ReadToEnd();
                            checkins = JsonConvert.DeserializeObject<FoursquareVenuesHistoryResponse>(responseHistory);
                        }
                    }

                    url = APICalls.getFoursquareRecentCheckins(user.FoursquareId);
                    request = WebRequest.Create(url);

                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            responseRecent = new StreamReader(stream).ReadToEnd();
                            recent = JsonConvert.DeserializeObject<FoursquareRecentCheckinsResponse>(responseRecent);
                        }
                    }

                    updateJsonData(userId, responseHistory, responseRecent);
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

        public static FoursquareStatistics getStatistics(FoursquareProfile profile)
        {
            try
            {
                String history = profile.UserCheckins;
                String recent = profile.Recent;
                FoursquareVenuesHistoryResponse userCheckins = JsonConvert.DeserializeObject<FoursquareVenuesHistoryResponse>(history);
                FoursquareRecentCheckinsResponse recentCheckins = JsonConvert.DeserializeObject<FoursquareRecentCheckinsResponse>(recent);
                FoursquareDataMining dataMining = new FoursquareDataMining(userCheckins, recentCheckins);
                FoursquareStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static FoursquareProfile getFoursquareProfileData(String userId)
        {
            FoursquareProfile profile = FoursquareDao.getProfileByUserId(userId);

            if (profile == null)
                return null;
            else
                return profile;
        }

        public static void updateJsonData(String userId, String jsonUserCheckins, String jsonRecent)
        {
            FoursquareProfile profile = FoursquareDao.getProfileByUserId(userId);
            if (profile == null)
            {
                FoursquareDao.addNewProfile(userId, jsonUserCheckins, jsonRecent);
            }
            else
            {
                FoursquareDao.updateProfile(userId, jsonUserCheckins, jsonRecent);
            }
        }

        public static void persistVenueCategories(FoursquareVenuesCategoriesResponse model)
        {
            foreach(Categories category in model.Categories.Categories)
            {
                FoursquareDao.Categories_addNew(category.Id, category.Name, 1, null);

                if (category.ListCategories != null)
                {
                
                    foreach(Categories subcategory in category.ListCategories)
                    {
                        FoursquareDao.Categories_addNew(subcategory.Id, subcategory.Name, 1, category.Id);
                    }
                }
            }
        }
    }
}