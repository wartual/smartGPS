using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using smartGPS.Business.Models.GoogleModels;

namespace smartGPS.Areas.Dashboard.Models
{
    public class DirectionsModel
    {
        [Display(Name="Start address")]
        public String StartAddress { get; set; }

        [Display(Name = "End address")]
        public String EndAddress { get; set; }

        public Location StartLocation { get; set; }

        public Location EndLocation { get; set; }

        [Display(Name = "Duration")]
        public String DurationText { get; set; }

        [Display(Name = "Distance")]
        public String DistanceText { get; set; }

        public double DurationValue { get; set; }

        public double DistanceValue { get; set; }

        public List<String> Directions { get; set; }

        [Display(Name = "Mode")]
        public int Mode { get; set; } 
       
        public IEnumerable<SelectListItem> Modes {get; set;}
        
    }
}