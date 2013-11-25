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
        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        [Display(Name = "Destination")]
        public String Destination { get; set; }
        
        public String NavigationType { get; set; }
        
        public int? DestinationDropdown { get; set; }
    }
}