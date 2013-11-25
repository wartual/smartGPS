using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Dashboard.Models
{
    public class PreviewTravelModel
    {

        [Display(Name = "Start address")]
        public String StartAddress { get; set; }

        [Display(Name = "End address")]
        public String EndAddress { get; set; }

        [Display(Name = "Distance")]
        public String Distance { get; set; }

        [Display(Name = "Duration")]
        public String Duration { get; set; }

        [Display(Name = "Mode")]
        public String Mode { get; set; }

    }
}