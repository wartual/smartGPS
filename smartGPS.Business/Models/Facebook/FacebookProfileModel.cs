using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace smartGPS.Business.Models.Facebook
{
    public class FacebookProfileModel
    {
        [JsonProperty("id")]
        public String id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("first_name")]
        public String FirstName { get; set; }

        [JsonProperty("last_name")]
        public String Surname { get; set; }

        [JsonProperty("middle_name")]
        public String MiddleName { get; set; }

        [JsonProperty("gender")]
        public String Gender { get; set; }

        [JsonProperty("link")]
        public String Link { get; set; }

        [JsonProperty("username")]
        public String Username { get; set; }

        [JsonProperty("bio")]
        public String Biography { get; set; }

        [JsonProperty("birthday")]
        public String Birthday { get; set; }

        [JsonProperty("friends")]
        public UserFriends UserFriends { get; set; }

        [JsonProperty("music")]
        public UserMusic UserMusic { get; set; }

        [JsonProperty("likes")]
        public UserLikes UserLikes { get; set; }

        [JsonProperty("books.reads")]
        public UserBooks UserBooks { get; set; }

        [JsonProperty("video.watches")]
        public UserVideos UserVideos { get; set; }

        [JsonProperty("sports")]
        public List<Sport> Sports { get; set; }

        [JsonProperty("favorite_athletes")]
        public List<BasicInformation> FavoriteAthletes { get; set; }

        [JsonProperty("favorite_teams")]
        public List<BasicInformation> FavoriteTeams { get; set; }

        [JsonProperty("checkins")]
        public UserCheckins UserCheckins { get; set; }
      }
}