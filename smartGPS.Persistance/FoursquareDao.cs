using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class FoursquareDao:BaseClass
    {
        public static FoursquareProfile getProfileByUserId(String userId)
        {
            return db.FoursquareProfile.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }

        public static void addNewProfile(String userId, String userCheckins, String recent)
        {
            FoursquareProfile model = new FoursquareProfile();
            model.UserId = userId;
            model.UserCheckins = userCheckins;
            model.Recent = recent;
            model.DateCreated = DateTime.Now;
            model.DateUpdated = DateTime.Now;
            model.Id = Guid.NewGuid().ToString();

            db.FoursquareProfile.Add(model);
            db.SaveChanges();
        }

        public static void addNewProfile(FoursquareProfile profile)
        {
            db.FoursquareProfile.Add(profile);
            db.SaveChanges();
        }

        public static void updateProfile(String userId, String userCheckins, String recent)
        {
            FoursquareProfile profile = getProfileByUserId(userId);

            if (userCheckins != null)
            {
                profile.UserCheckins = userCheckins;
            }

            if (recent != null)
            {
                profile.Recent = recent;
            }

            profile.DateUpdated = DateTime.Now;
            db.SaveChanges();
        }
    }
}