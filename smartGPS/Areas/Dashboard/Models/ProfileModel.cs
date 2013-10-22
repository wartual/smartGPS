using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Dashboard.Models
{
    public class ProfileModel
    {
        [Display(Name = "Name")]
        public String name { get; set; }

        [Display(Name = "Surname")]
        public String surname { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String email { get; set; }

        [Display(Name = "Username")]
        public String username { get; set; }

        [Display(Name = "Password")]
        public String password { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime? dateOfBirth { get; set; }

        [Display(Name = "Address")]
        public String address { get; set; }

        [Display(Name = "Postal office")]
        public String postalOffice { get; set; }

        [Display(Name = "Country")]
        public String country { get; set; }

        [Display(Name = "Gender")]
        public Boolean? gender { get; set; }
    }
}