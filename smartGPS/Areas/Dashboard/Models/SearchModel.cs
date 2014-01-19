using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Dashboard.Models
{
    public class SearchModel
    {
        [Display(Name = "Place")]
        public String Place { get; set; }

        [Display(Name = "Latitude")]
        public String Latitude { get; set; }

        [Display(Name = "Longitude")]
        public String Longitude { get; set; }

        [Display(Name = "Search type")]
        public String SearchType { get; set; }

    }
}