using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using smartGPS.Business.Models.Facebook;

namespace smartGPS.Areas.Dashboard.Models
{
    public class FacebookStatisticsModel
    {

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Number of friends analyed")]
        public int CountFriendsAnalzyed { get; set; }

        [Display(Name = "Number of user likes")]
        public int CountUserLikes { get; set; }

        [Display(Name = "Number of friends likes")]
        public int CountFriendsLikes { get; set; }

        [Display(Name = "Most frequent user like category")]
        public String MostFrequendUserLikeCategory { get; set; }

        [Display(Name = "Most frequent user checkin")]
        public String MostFrequendUserCheckin { get; set; }

        [Display(Name = "Most frequent friend like category")]
        public String MostFrequendFriendLikeCategory { get; set; }

        [Display(Name = "Most frequent friend like")]
        public String MostFrequendFriendLike { get; set; }

        [Display(Name = "Most frequent friends checkins")]
        public String MostFrequendFriendCheckin { get; set; }

        [Display(Name = "List of similar friends by likes")]
        public String SimilarFriendsList { get; set; }

        [Display(Name = "Likes music?")]
        public String LikesMusic { get; set; }

        [Display(Name = "Likes books?")]
        public String LikesBooks { get; set; }

        [Display(Name = "Likes movies?")]
        public String LikesMovies { get; set; }

        [Display(Name = "Likes sports?")]
        public String LikesSports { get; set; }

        [Display(Name = "Likes traveling?")]
        public String LikesTravelling { get; set; }

        [Display(Name = "Is sportsman?")]
        public String IsSportsman { get; set; }

        public Dictionary<FacebookProfileModel, int> SimilarFriends { get; set; }
        public Dictionary<String, int> UserLikesCategoriesFrequency { get; set; }
        public Dictionary<String, int> FriendsLikesFrequency { get; set; }
        public Dictionary<String, int> FriendsLikesCategoriesFrequency { get; set; }
        public Dictionary<String, int> FriendsCheckinsFrequency { get; set; }
        public Dictionary<String, int> UserCheckinsFrequency { get; set; }
        public IEnumerable<KeyValuePair<String, int>> SortedFriendsLikesFrequency { get; set; }
        public IEnumerable<KeyValuePair<String, int>> SortedFriendsLikesCategoriesFrequency { get; set; }
        public IEnumerable<KeyValuePair<String, int>> SortedUserLikesCategoriesFrequency { get; set; }
        public IEnumerable<KeyValuePair<FacebookProfileModel, int>> SortedSimillarFriends { get; set; }
        public IEnumerable<KeyValuePair<String, int>> SortedFriendsCheckinsFrequency { get; set; }
        public IEnumerable<KeyValuePair<String, int>> SortedUserCheckinsFrequency { get; set; }
    }
}