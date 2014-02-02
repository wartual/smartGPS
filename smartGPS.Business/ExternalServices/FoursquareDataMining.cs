using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Models.Foursquare;

namespace smartGPS.Business.ExternalServices
{
    public class FoursquareDataMining
    {
        private Dictionary<String, int> checkinCategoriesFrequency;
        private Dictionary<String, int> checkinsFrequency;
        private Dictionary<String, int> visitedCitiesFrequency;
        private Dictionary<String, int> friendsCheckinCategoriesFrequency;
        private Dictionary<String, int> friendsCheckinsFrequency;
        private FoursquareVenuesHistoryResponse model;
        private FoursquareRecentCheckinsResponse recentCheckins;
        private FoursquareStatistics statistics;
        private String name { get; set; }
    
        public FoursquareDataMining(FoursquareVenuesHistoryResponse model, FoursquareRecentCheckinsResponse recentCheckins)
        {
            this.model = model;
            this.recentCheckins = recentCheckins;
        }

        public FoursquareStatistics analyze()
        {
            analyzeCheckinsCategories();
            analyzeVisitedCities();
            analyzeCheckins();
            analyzeFriendsCheckins();

            prepareStatistics();
            return statistics;
        }

        private void analyzeFriendsCheckins()
        {
            friendsCheckinCategoriesFrequency = new Dictionary<string, int>();
            friendsCheckinsFrequency = new Dictionary<string, int>();

            Recent checkin;
            int value;
            Categories category;

            for (int i = 0; i < recentCheckins.Response.RecentCheckings.Count; i++)
            {
                checkin = recentCheckins.Response.RecentCheckings.ElementAt(i);
                if (!checkin.User.Relationship.Equals(User.SELF))
                {
                    // checkin
                    if (friendsCheckinsFrequency.ContainsKey(checkin.Venue.Name))
                    {
                        value = friendsCheckinsFrequency[checkin.Venue.Name];
                        friendsCheckinsFrequency[checkin.Venue.Name] = value + 1;
                    }
                    else
                    {
                        friendsCheckinsFrequency.Add(checkin.Venue.Name, 1);
                    }

                    // categories
                    for (int j = 0; j < checkin.Venue.Categories.Count; j++)
                    {
                        category = checkin.Venue.Categories.ElementAt(j);
                        if (friendsCheckinCategoriesFrequency.ContainsKey(category.Name))
                        {
                            value = friendsCheckinCategoriesFrequency[category.Name];
                            friendsCheckinCategoriesFrequency[category.Name] = value + 1;
                        }
                        else
                        {
                            friendsCheckinCategoriesFrequency.Add(category.Name, 1);
                        }
                    }
                }
                else
                {
                    if (name == null)
                    {
                        name = checkin.User.FullName;
                    }
                }
            }
        }

        private void analyzeCheckins()
        {
            checkinsFrequency = new Dictionary<String, int>();
            CheckinItems checkin;

            int value;

            for (int i = 0; i < model.Response.Checkins.Count; i++)
            {
                checkin = model.Response.Checkins.CheckinItems.ElementAt(i);
               
                if (checkinsFrequency.ContainsKey(checkin.Venue.Name))
                {
                    value = checkinsFrequency[checkin.Venue.Name];
                    checkinsFrequency[checkin.Venue.Name] = value + 1;
                }
                else
                {
                    checkinsFrequency.Add(checkin.Venue.Name, 1);
                }
            }
        }

        private void analyzeVisitedCities()
        {
            visitedCitiesFrequency = new Dictionary<string, int>();
            CheckinItems checkin;
            Location location;
            String city;
            int value;

            for (int i = 0; i < model.Response.Checkins.Count; i++)
            {
                checkin = model.Response.Checkins.CheckinItems.ElementAt(i);
                location = checkin.Venue.Location;
                city = location.City + ", " + location.State;

                if (visitedCitiesFrequency.ContainsKey(city))
                {
                    value = visitedCitiesFrequency[city];
                    visitedCitiesFrequency[city] = value + 1;
                }
                else
                {
                    visitedCitiesFrequency.Add(city, 1);
                }
            }
        }

        private void analyzeCheckinsCategories()
        {
            checkinCategoriesFrequency = new Dictionary<string, int>();
            CheckinItems checkin;
            Categories category;
            int value;
            for (int i = 0; i < model.Response.Checkins.Count; i++)
            {
                 checkin = model.Response.Checkins.CheckinItems.ElementAt(i);
                 for (int j = 0; j < checkin.Venue.Categories.Count; j++)
                 {
                     category = checkin.Venue.Categories.ElementAt(j);
                     if (checkinCategoriesFrequency.ContainsKey(category.Name))
                     {
                         value = checkinCategoriesFrequency[category.Name];
                         checkinCategoriesFrequency[category.Name] = value + 1;
                     }
                     else
                     {
                         checkinCategoriesFrequency.Add(category.Name, 1);
                     }
                 }
                
            }
        }

        private void prepareStatistics()
        {
            statistics = new FoursquareStatistics();
            statistics.Name = name;
            statistics.CheckinCategoriesFrequency = checkinCategoriesFrequency;
            statistics.CheckinsFrequency = checkinsFrequency;
            statistics.FriendsCheckinCategoriesFrequency = friendsCheckinCategoriesFrequency;
            statistics.FriendsCheckinsFrequency = friendsCheckinsFrequency;
            statistics.VisitedCitiesFrequency = visitedCitiesFrequency;

            statistics.SortedCheckinCategoriesFrequency = Utilities.returnSortedKeyValuePair(checkinCategoriesFrequency);
            statistics.SortedCheckinsFrequency = Utilities.returnSortedKeyValuePair(checkinsFrequency);
            statistics.SortedFriendsCheckinCategoriesFrequency = Utilities.returnSortedKeyValuePair(friendsCheckinCategoriesFrequency);
            statistics.SortedFriendsCheckinsFrequency = Utilities.returnSortedKeyValuePair(friendsCheckinsFrequency);
            statistics.SortedVisitedCitiesFrequency = Utilities.returnSortedKeyValuePair(visitedCitiesFrequency);
        }
    }
}