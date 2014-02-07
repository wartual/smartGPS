using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using smartGPS.Business.CBA;
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
                var fb = client.Get("me?fields=id,name,sports,location,favorite_athletes,favorite_teams,interests,music,video.watches,books.reads,friends.limit(200).fields(id,name, sports,location,favorite_athletes,favorite_teams,interests,music,video.watches,books.reads)");
                FacebookProfileModel model = JsonConvert.DeserializeObject<FacebookProfileModel>(fb.ToString());

                updateJsonData(userId, fb.ToString().Trim(), null, null);

           
                FacebookDataMining dataMining = new FacebookDataMining(model, null, null);
                FacebookStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static FacebookStatistics importLikes(String token, String userId)
        {
            FacebookClient client = new FacebookClient(token);

            try
            {
                var fb = client.Get("me?fields=id,name,likes,friends.limit(200).fields(likes,id,name)");
                FacebookProfileModel model = JsonConvert.DeserializeObject<FacebookProfileModel>(fb.ToString());

                updateJsonData(userId, null, null, fb.ToString().Trim());


                FacebookDataMining dataMining = new FacebookDataMining(null, model, null);
                FacebookStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static FacebookStatistics importCheckins(String token, String userId)
        {
            FacebookClient client = new FacebookClient(token);

            try
            {
                var fb = client.Get("me?fields=id,name,checkins,friends.limit(200).fields(checkins,id,name)");
                FacebookProfileModel model = JsonConvert.DeserializeObject<FacebookProfileModel>(fb.ToString());

                updateJsonData(userId, null, fb.ToString().Trim(), null);

                FacebookDataMining dataMining = new FacebookDataMining(null, null, model);
                FacebookStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static FacebookStatistics getStatistics(FacebookProfile profile)
        {
            try
            {
                FacebookProfileModel basicInfo = JsonConvert.DeserializeObject<FacebookProfileModel>(profile.JsonPersonalDataAndFriends);
                FacebookProfileModel checkins = JsonConvert.DeserializeObject<FacebookProfileModel>(profile.JsonUserAndFriendsCheckins);

                FbProccessDataForPersitence processData = new FbProccessDataForPersitence(basicInfo, checkins, profile.UserId);
                processData.proccessData();
                
                FacebookProfileModel likes = JsonConvert.DeserializeObject<FacebookProfileModel>(profile.JsonUserAndFriendsLikes);
                FacebookDataMining dataMining = new FacebookDataMining(basicInfo, likes, checkins);
                FacebookStatistics statistics = dataMining.analyze();

                return statistics;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void updateProccessedEntries(FacebookProfileModel basicInfo, String userId)
        {
           
        }

        public static FacebookProfile getFacebookProfileData(String userId)
        {
            FacebookProfile profile = FacebookDao.getProfileByUserId(userId);

            if (profile == null)
                return null;
            else
                return profile;
        }

        public static void updateJsonData(String userId, String jsonPersonalAndFriends, String jsonUserAndFriendsCheckins, String jsonUserAndFriendsLikes)
        {
            FacebookProfile profile = FacebookDao.getProfileByUserId(userId);
            if (profile == null)
            {
                FacebookDao.addNewProfile(userId, jsonPersonalAndFriends, jsonUserAndFriendsCheckins, jsonUserAndFriendsLikes);
            }
            else
            {
                FacebookDao.updateProfile(userId, jsonPersonalAndFriends, jsonUserAndFriendsCheckins, jsonUserAndFriendsLikes);
            }
        }

        #region Utils

       

        #endregion
    }
}