using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance.Users
{
    public class ExternalServicesDAO : BaseClass
    {
        
        public static void addFacebookProfile(String userId, String name, String lastName, String middleName, String username, String link,
                        String gender, String birthday, String biography)
        {
            FacebookProfile model = new FacebookProfile();

            model.UserFacebookProfileId = Guid.NewGuid().ToString();
            model.UserId = userId;
            model.Name = name;
            model.Surname = lastName;
            model.MiddleName = middleName;
            model.Username = username;
            model.Link = link;
            if(birthday != null)
              model.DateOfBirth = DateTime.ParseExact(birthday, "MM/dd/YYYY", null);
            model.Biography = biography;

            model.DateCreated = DateTime.Now;
            model.DateUpdated = DateTime.Now;

            db.FacebookProfile.Add(model);
            db.SaveChanges();
        }

        public static FacebookProfile getFacebookProfileByUserId(String userId)
        {
            return db.FacebookProfile.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }
    }
}