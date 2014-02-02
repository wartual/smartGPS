using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.Models.Facebook
{
    public class FacebookStatistics
    {

        public String Name { get; set; }
        public Dictionary<String, int> UserLikesFrequency{get; set;}
        public Dictionary<String, int> UserLikesCategoriesFrequency { get; set; }
        public Dictionary<String, int> FriendsLikesFrequency{get; set;}
        public Dictionary<String, int> FriendsLikesCategoriesFrequency { get; set; }
        public Dictionary<FacebookProfileModel, int> similarFriends { get; set; }
        public int CountFriendsAnalzyed { get; set; }
        public int CountFriendsLikes { get; set; }
        public int CountUserLikes { get; set; }
        public List<KeyValuePair<String, int>> SortedUserLikesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedUserLikesCategoriesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsLikesFrequency { get; set; }
        public List<KeyValuePair<String, int>> SortedFriendsLikesCategoriesFrequency { get; set; }
        public List<KeyValuePair<FacebookProfileModel, int>> SortedSimillarFriends { get; set; }

        public int likesMusic { get; set; }
        public int likesBooks { get; set; }
        public int likesMovies { get; set; }
        public int isSportsman { get; set; }
        public int likesSports { get; set; }
        public int likesTraveling { get; set; }
    }
}