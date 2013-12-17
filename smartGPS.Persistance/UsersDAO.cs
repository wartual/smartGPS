using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance.UsersFolder
{
    public class UsersDAO:BaseClass
    {

        #region Users

        public static void addNew(String id, String username, String password, String name, String surname, String facebookId, String twitterId)
        {
            User model = new User();
            model.Id = id;
            model.Username = username;
            model.Password = password;
            model.FacebookId = facebookId;
            model.TwitterId = twitterId;
            model.DateCreated = DateTime.Now;
            model.DateLastLogin = DateTime.Now;
         
            Profile profile = new Profile();
            profile.Id = Guid.NewGuid().ToString();
            profile.Name = name;
            profile.Surname = surname;

            UserHelper helper = new UserHelper();
            helper.Id = Guid.NewGuid().ToString();
            helper.DateUpdated = DateTime.Now;

            model.UserHelper.Add(helper);
            model.Profile.Add(profile);
            db.User.Add(model);
            db.SaveChanges();
        }

        public static User getById(String id)
        {
            return db.User.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static User getByUsername(String username)
        {
            return db.User.Include("profile").Where(item => item.Username.Equals(username)).SingleOrDefault();
        }

        public static User getByUsernameAndPassword(String username, String password)
        {
            return db.User.Where(item => item.Username.Equals(username) && item.Password.Equals(password)).SingleOrDefault();
        }

        public static Boolean alreadyExists(String username) 
        {
            User model = db.User.Where(item => item.Username.Equals(username)).SingleOrDefault();
            if (model == null)
                return false;
            else
                return true;
        }

        public static void updateDateLastLogin(User model)
        {
            model.DateLastLogin = DateTime.Now;
            db.SaveChanges();
        }

#endregion


        #region Profile

        public static Profile getProfileById(String id)
        {
            return db.Profile.Include("User").Where(m => m.Id.Equals(id)).SingleOrDefault();
        }

        public static Profile getProfileByUserId(String userId)
        {
            return db.Profile.Include("User").Where(m => m.User.Id.Equals(userId)).SingleOrDefault();
        }

        public static Boolean updateProfile(String userId, String username, String name, String surname, DateTime? dateofBirth, Boolean? gender, String email,
                                               String phone, String address, String postalOffice, String country, DateTime? date)
        {
            Profile profile = getProfileByUserId(userId);

            if(profile == null){
                return false;
            }

            profile.Address = address;
            profile.Country = country;

            if (date.HasValue)
            {
                profile.DateOfBirth = date.Value;
            }
            else if(dateofBirth.HasValue)
            {
                profile.DateOfBirth = dateofBirth.Value;
            }

            profile.Email = email;
            profile.Name = name;
            profile.Phone = phone;
            profile.PostalOffice = postalOffice;
            profile.Surname = surname;

            db.SaveChanges();

            return true;
        }

        #endregion


        #region Helper

        public static UserHelper getUserHelper(String userId)
        {
            return db.UserHelper.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }

        public static void updateLastLocation(double latitude, double longitude, String userId)
        {
            UserHelper model = getUserHelper(userId);

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
