using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance.Users
{
    public class UsersDAO:BaseClass
    {
        public static void addNew(String id, String username, String password, String name, String surname, String facebookId, String twitterId)
        {
            users model = new users();
            model.Id = id;
            model.Username = username;
            model.Password = password;
            model.FacebookId = facebookId;
            model.TwitterId = twitterId;
            model.DateCreated = DateTime.Now;
            model.DateLastLogin = DateTime.Now;

            profile profile = new profile();
            profile.Id = Guid.NewGuid().ToString();
            profile.Name = name;
            profile.Surname = surname;

            usershelper helper = new usershelper();
            helper.Id = Guid.NewGuid().ToString();
            helper.DateUpdated = DateTime.Now;

            model.usershelper.Add(helper);
            model.profile.Add(profile);
            db.users.Add(model);
            db.SaveChanges();
        }

        public static users getById(String id)
        {
            return db.users.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static users getByUsername(String username)
        {
            return db.users.Include("profile").Where(item => item.Username.Equals(username)).SingleOrDefault();
        }

        public static users getByUsernameAndPassword(String username, String password)
        {
            return db.users.Where(item => item.Username.Equals(username) && item.Password.Equals(password)).SingleOrDefault();
        }

        public static Boolean alreadyExists(String username) 
        {
            users model = db.users.Where(item => item.Username.Equals(username)).SingleOrDefault();
            if (model == null)
                return false;
            else
                return true;
        }

        public static void updateDateLastLogin(users model)
        {
            model.DateLastLogin = DateTime.Now;
            db.SaveChanges();
        }

        #region Profile

        public static profile getProfileById(String id)
        {
            return db.profile.Include("users").Where(m => m.Id.Equals(id)).SingleOrDefault();
        }

        public static profile getProfileByUserId(String userId)
        {
            return db.profile.Include("users").Where(m => m.users.Id.Equals(userId)).SingleOrDefault();
        }

        #endregion

        #region Helper

        public static usershelper getUserHelper(String userId)
        {
            return db.usershelper.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }

        public static void updateLastLocation(double latitude, double longitude, String userId)
        {
            usershelper model = getUserHelper(userId);

            if (model != null)
            {
                model.LastLocationLatitude = latitude;
                model.LastLocationLongitude = longitude;
                db.SaveChanges();
            }
        }

        #endregion
    }
}
