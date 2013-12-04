using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using Newtonsoft.Json;
using smartGPS.Business.Models;
using smartGPS.Persistance;
using smartGPS.Persistance.Users;

namespace smartGPS.Business.ExternalServices
{
    public class FacebookManagement
    {

        public static Boolean importFacebookData(String token, String userId)
        {
            FacebookClient client = new FacebookClient(token);
            try
            {
                String me = client.Get("me").ToString();
                FacebookProfileModel model = JsonConvert.DeserializeObject<FacebookProfileModel>(me);
                ExternalServicesDAO.addFacebookProfile(userId, model.Name, model.Surname, model.MiddleName, model.Username, model.Link,
                                                model.Gender, model.Birthday, model.Biography);
                return true;
            }
            catch (Exception e)
            {
                return false;
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