using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using smartGPS.Custom.Attributes;

namespace smartGPS.Areas.Dashboard.Models
{
    public class SetupTravelModel
    {
        [Display(Name = "Departure address")]
        public String StartAddress { get; set; }

        [Display(Name = "Departure latitude")]
        public String StartLatitude { get; set; }

        [Display(Name = "Departure longitude")]
        public String StartLongitude { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Latitude")]
        public String Latitude { get; set; }

        [Display(Name = "Longitude")]
        public String Longitude { get; set; }

        [Display(Name = "Departure")]
        public String Departure { get; set; }

        [Display(Name = "Destination")]
        public String Destination { get; set; }

        [Display(Name = "Select departure type")]
        public String DepartureType { get; set; }
        
        [Display(Name = "Select destination type")]
        public String DestinationType { get; set; }

        [Display(Name = "Destination")]
        public int? DestinationDropdown { get; set; }

        [Display(Name = "Departure")]
        public int? DepartureDropdown { get; set; }
    }
}