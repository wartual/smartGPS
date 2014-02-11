using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business.Models.Facebook
{
    public class FbProccessDataForPersitence
    {

        private List<UserFriends> Friends { get; set; }
        private FacebookProfileModel BasicInfo { get; set; }
        private FacebookProfileModel Checkins { get; set; }
        private String UserId { get; set; }

        public FbProccessDataForPersitence(FacebookProfileModel basicInfo, FacebookProfileModel checkins, String userId)
        {
            this.BasicInfo = basicInfo;
            this.UserId = userId;
            this.Checkins = checkins;
        }

        public void proccessData()
        {
            FacebookDao.ProccessedFacebookEntries_deleteAllForUser(UserId);
            int likesSports;
            int i = 0;
            foreach(FacebookProfileModel model in BasicInfo.UserFriends.Friends)
            {
                if(analyzeSports(model) == (int)CommonModels.Status.True || analyzeFavoriteTeams(model) == (int)CommonModels.Status.True || analyzeFavoriteAthletes(model) == (int)CommonModels.Status.True)
                {
                    likesSports = (int)CommonModels.Status.True;
                }
                else if(analyzeSports(model) == (int)CommonModels.Status.False && analyzeFavoriteTeams(model) == (int)CommonModels.Status.False && analyzeFavoriteAthletes(model) == (int)CommonModels.Status.False)
                    likesSports = (int) CommonModels.Status.False;
                else
                    likesSports = (int) CommonModels.Status.Unknown;

                FacebookDao.ProccessedFacebookEntries_addNew(Guid.NewGuid().ToString(), UserId, model.Name, Utilities.mapStatusEnumToWord(analyzeSports(model)), Utilities.mapStatusEnumToWord(analyzeMusic(model)), Utilities.mapStatusEnumToWord(analyzeBooks(model)), Utilities.mapStatusEnumToWord(analyzeVideos(model)), Utilities.mapStatusEnumToWord(analyzeMyCheckins(i)), Utilities.mapStatusEnumToWord(likesSports));
                i++;
            }
        }

        public int analyzeMusic(FacebookProfileModel model)
        {
            if (model.UserMusic != null)
            {
                if (model.UserMusic.Music.Count >= Config.LIKES_MUSIC_LIMIT)
                {
                    return (int)CommonModels.Status.True;
                }
                else
                {
                    return (int)CommonModels.Status.False;
                }
            }
            else
            {
                return (int)CommonModels.Status.Unknown;
            }
        }

        public int analyzeBooks(FacebookProfileModel model)
        {
            if (model.UserBooks != null)
            {
                if (model.UserBooks.BookData.Count >= Config.LIKES_BOOK_LIMIT)
                {
                    return (int)CommonModels.Status.True;
                }
                else
                {
                    return (int)CommonModels.Status.False;
                }
            }
            else
            {
                return (int)CommonModels.Status.Unknown;
            }
        }

        public int analyzeVideos(FacebookProfileModel model)
        {
            if (model.UserVideos != null)
            {
                if (model.UserVideos.VideosData.Count >= Config.LIKES_MOVIES_LIMIT)
                {
                    return (int)CommonModels.Status.True;
                }
                else
                {
                    return (int)CommonModels.Status.False;
                }
            }
            else
            {
                return (int)CommonModels.Status.Unknown;
            }
        }

        public int analyzeSports(FacebookProfileModel model)
        {
            if (model.Sports != null)
            {
                return (int)CommonModels.Status.True;
            }
            else
            {
                return (int)CommonModels.Status.False;
            }
        }

        public int analyzeFavoriteAthletes(FacebookProfileModel model)
        {
            if (model.FavoriteAthletes != null)
            {
                if (model.FavoriteAthletes.Count >= Config.LIKES_SPORTS)
                {
                    return (int)CommonModels.Status.True;
                }
                else
                {
                    return (int)CommonModels.Status.False;
                }

            }
            else
            {
                return (int)CommonModels.Status.Unknown;
            }
        }

        public int analyzeFavoriteTeams(FacebookProfileModel model)
        {
            if (model.FavoriteTeams != null)
            {
                if (model.FavoriteTeams.Count >= Config.LIKES_SPORTS)
                {
                    return (int)CommonModels.Status.True;
                }
                else
                {
                    return (int)CommonModels.Status.False;
                }

            }
            else
            {
                return (int)CommonModels.Status.Unknown;
            }
        }

        private int analyzeMyCheckins(int index)
        {
            Dictionary<string, int> myCheckins = new Dictionary<string, int>();
            CheckinData place;
            int value;

            FacebookProfileModel model = Checkins.UserFriends.Friends.ElementAt(index);

            if (model.UserCheckins != null)
            {
                for (int i = 0; i < model.UserCheckins.Checkins.Count(); i++)
                {
                    place = model.UserCheckins.Checkins.ElementAt(i);
                    if (myCheckins.ContainsKey(place.Checkins.Name))
                    {
                        value = myCheckins[place.Checkins.Name];
                        myCheckins[place.Checkins.Name] = value + 1;
                    }
                    else
                    {
                        myCheckins.Add(place.Checkins.Name, 1);
                    }
                }

                if (myCheckins.Count >= 3)
                {
                    return (int)CommonModels.Status.True;
                }
                else 
                {
                    return (int)CommonModels.Status.False;
                }
            }          
            else
                return (int)CommonModels.Status.Unknown;
        }
    }
}