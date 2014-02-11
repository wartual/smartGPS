using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class FacebookDao : BaseClass
    {

        public static FacebookProfile getProfileByUserId(String userId)
        {
            return db.FacebookProfile.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }

        public static void addNewProfile(String userId, String jsonPersonalAndFriends, String jsonUserAndFriendsCheckins, String jsonUserAndFriendsLikes)
        {
            FacebookProfile model = new FacebookProfile();
            model.UserId = userId;
            model.JsonPersonalDataAndFriends = jsonPersonalAndFriends;
            model.JsonUserAndFriendsCheckins = jsonUserAndFriendsCheckins;
            model.JsonUserAndFriendsLikes = jsonUserAndFriendsLikes;
            model.DateCreated = DateTime.Now;
            model.DateUpdated = DateTime.Now;
            model.Id = Guid.NewGuid().ToString();

            db.FacebookProfile.Add(model);
            db.SaveChanges();
        }

        public static void addNewProfile(FacebookProfile profile)
        {
            db.FacebookProfile.Add(profile);
            db.SaveChanges();
        }

        public static void updateProfile(String userId, String jsonPersonalAndFriends, String jsonUserAndFriendsCheckins, String jsonUserAndFriendsLikes)
        {
            FacebookProfile profile = getProfileByUserId(userId);

            if (jsonPersonalAndFriends != null)
            {
                profile.JsonPersonalDataAndFriends = jsonPersonalAndFriends;
            }

            if (jsonUserAndFriendsCheckins != null)
            {
                profile.JsonUserAndFriendsCheckins = jsonUserAndFriendsCheckins;
            }

            if (jsonUserAndFriendsLikes != null)
            {
                profile.JsonUserAndFriendsLikes = jsonUserAndFriendsLikes;
            }

            profile.DateUpdated = DateTime.Now;
            db.SaveChanges();
        }

        #region ProcessedFacebookEntries

        public static void ProccessedFacebookEntries_addNew(String id, String userId, String userName, String sportsman, String likesMusic, String likesBook, String likesMovies, String likesTravelling, String likesSports)
        {
            FacebookProccesedEntries model = new FacebookProccesedEntries();
            Random random = new Random();
            int category = random.Next(0, 2);
            
            model.Id = id;
            model.LikesBooks = likesBook;
            model.LikesMovies = likesMovies;
            model.LikesMusic = likesMusic;
            model.LikesSports = likesSports;
            model.LikesTravelling = likesTravelling;
            model.Sportsman = sportsman;
            model.UserId = userId;
            model.UserName = userName;
            model.DateCreated = DateTime.Now;

            model.Category = category;
            db.FacebookProccesedEntries.Add(model);
            db.SaveChanges();
        }

        public static void ProccessedFacebookEntries_deleteAllForUser(String userId)
        {
            IEnumerable<FacebookProccesedEntries> entries = ProccessedFacebookEntries_getAllByUser(userId);

            if (entries != null && entries.Count() != 0)
            {
                foreach (FacebookProccesedEntries entry in entries)
                {
                    db.FacebookProccesedEntries.Remove(entry);
                }

                db.SaveChanges();
            }
        }

        public static List<FacebookProccesedEntries> ProccessedFacebookEntries_getAllByUser(String userId)
        {
            return db.FacebookProccesedEntries.Where(item => item.UserId.Equals(userId)).ToList();
        }

        public static String mapEnumCategoryToWord(int enumerator)
        {
            if (enumerator == 0)
                return "Traveller";
            else if (enumerator == 1)
                return "Sportsman";
            else
                return "Music";
        }

        #endregion
    }
}