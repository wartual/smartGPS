using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Facebook;
using smartGPS.Persistance;
using smartGPS.Persistance.Users;

namespace smartGPS.Business.ExternalServices
{
    public class FacebookManagement
    {
        public static FacebookStatistics importFacebookData(String token, String userId)
        {
            FacebookClient client = new FacebookClient(token);
            
            try
            {
                var fb = client.Get("me?fields=id,name,sports,likes,location,favorite_athletes,favorite_teams,interests,music,video.watches,checkins,books.reads,friends.limit(200).fields(likes,id,name)");
                FacebookProfileModel model = JsonConvert.DeserializeObject<FacebookProfileModel>(fb.ToString());
                FacebookDataMining dataMining = new FacebookDataMining(model);
                FacebookStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public static FacebookProfileModel getFacebookProfileData(String userId)
        {
            FacebookProfile profile = ExternalServicesDAO.getFacebookProfileByUserId(userId);
            FacebookProfileModel model = new FacebookProfileModel();

            if (profile == null)
                return null;
            else
                return Mapping.mapFacebookDBModelToFacebookProfileModel(profile, model);
        }
    }
}