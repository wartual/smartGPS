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
        public String Name { get; set; }

        [Display(Name = "Surname")]
        public String Surname { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "Username")]
        public String Username { get; set; }

        [Display(Name = "Password")]
        public String Password { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Phone")]
        public String Phone { get; set; }

        [Display(Name = "Postal office")]
        public String PostalOffice { get; set; }

        [Display(Name = "Country")]
        public String Country { get; set; }

        [Display(Name = "Category")]
        public String Category { get; set; }

        [Display(Name = "Gender")]
        public Boolean? Gender { get; set; }

        public FacebookStatisticsModel FacebookStatistics { get; set; }
        public FoursquareStatisticsModel FoursquareStatistics { get; set; }
    }
}