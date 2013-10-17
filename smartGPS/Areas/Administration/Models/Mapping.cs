using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Areas.Dashboard.Models;
using smartGPS.Persistence;

namespace smartGPS.Areas.Administration.Models
{
    public class Mapping
    {
        // <summary>
        // Maps user from db to ProfileModel
        // </summary>
        public static ProfileModel usersToProfileModel(profile profile)
        {
            ProfileModel model = new ProfileModel();
            model.name = profile.Name;
            model.surname = profile.Surname;
            model.username = profile.users.Username;
            model.address = profile.Address;
            model.country = profile.Country;
            model.dateOfBirth = profile.DateOfBirth;
            model.gender = profile.Gender;
            model.postalOffice = profile.PostalOffice;

            return model;
        }
    }
}