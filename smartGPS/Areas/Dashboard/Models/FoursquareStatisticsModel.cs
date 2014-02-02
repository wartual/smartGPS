using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace smartGPS.Areas.Dashboard.Models
{
    public class FoursquareStatisticsModel
    {
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Most frequent user checkins")]
        public String MostFrequentUserCheckins { get; set; }

        [Display(Name = "Most frequend user checkins by category")]
        public String MostFrequendUserCheckinsByCategory { get; set; }

        [Display(Name = "Most frequend user visited places")]
        public String MostFrequendUserVisitedPlaces { get; set; }

        public List<KeyValuePair<String, int>> SortedCheckinCategoriesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedCheckinsFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedVisitedCitiesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsCheckinCategoriesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsCheckinsFrequency { get; set; }
    }
}