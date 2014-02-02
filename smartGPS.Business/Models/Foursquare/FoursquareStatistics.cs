using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.Models.Foursquare
{
    public class FoursquareStatistics
    {

        public String Name { get; set; }
        public Dictionary<String, int> CheckinCategoriesFrequency;
        public Dictionary<String, int> CheckinsFrequency;
        public Dictionary<String, int> VisitedCitiesFrequency;
        public Dictionary<String, int> FriendsCheckinCategoriesFrequency;
        public Dictionary<String, int> FriendsCheckinsFrequency;

        public List<KeyValuePair<String, int>> SortedCheckinCategoriesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedCheckinsFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedVisitedCitiesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsCheckinCategoriesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsCheckinsFrequency { get; set; }
    }
}