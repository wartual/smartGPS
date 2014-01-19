using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Persistance;

namespace smartGPS.Areas.Administration.Models
{
    public class Mapping
    {
        // <summary>
        // Maps user from db to ProfileModel
        // </summary>
        public static ProfileModel usersToProfileModel(Profile profile)
        {
            ProfileModel model = new ProfileModel();
            model.Name = profile.Name;
            model.Surname = profile.Surname;
            model.Username = profile.User.Username;
            model.Address = profile.Address;
            model.Country = profile.Country;
            model.DateOfBirth = profile.DateOfBirth;
            model.Gender = profile.Gender;
            model.PostalOffice = profile.PostalOffice;

            return model;
        }
    }
}