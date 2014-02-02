using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Models.Facebook;
using smartGPS.Business.Models.GoogleModels;
using smartGPS.Persistance;

namespace smartGPS.Business.Models
{
    public class Mapping
    {

        public static FacebookProfileModel mapFacebookDBModelToFacebookProfileModel(FacebookProfile db, FacebookProfileModel model)
        {
            model.Username = db.Username;
            model.Surname = db.Surname;
            model.Name = db.Name;
            model.MiddleName = db.MiddleName;
            model.Link = db.Link;
            model.Birthday = db.DateOfBirth.ToString();
            model.Biography = db.Biography;
            return model;
        }

        
    }
}