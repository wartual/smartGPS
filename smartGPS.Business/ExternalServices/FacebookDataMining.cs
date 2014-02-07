using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Facebook;

namespace smartGPS.Business.ExternalServices
{
    public class FacebookDataMining
    {
        private Dictionary<String, int> friendslikesFrequency;
        private Dictionary<String, int> myLikesFrequency;
        private Dictionary<String, int> friendslikesCategoriesFrequency;
        private Dictionary<String, int> myLikesCategoriesFrequency;
        private Dictionary<FacebookProfileModel, int> similarFriends;
        private Dictionary<String, int> friendsCheckinsFrequency;
        private Dictionary<String, int> myCheckins;
        private FacebookProfileModel basicInfo;
        private FacebookProfileModel checkins;
        private FacebookProfileModel likes;
        private FacebookStatistics statistics;
        private int likesMusic;
        private int likesBooks;
        private int likesMovies;
        private int isSportsman;
        private int likesSports;
        private int likesTraveling;

        public FacebookDataMining(FacebookProfileModel basicInfo, FacebookProfileModel likes, FacebookProfileModel checkins)
        {
            this.basicInfo = basicInfo;
            this.checkins = checkins;
            this.likes = likes;
        }

        public FacebookStatistics analyze()
        {
            similarFriends = new Dictionary<FacebookProfileModel, int>();
            friendsCheckinsFrequency = new Dictionary<string, int>();
            myCheckins = new Dictionary<string, int>();

            if (basicInfo != null)
            {
                analyzeMusic();
                analyzeBooks();
                analyzeVideos();
                analyzeSports();
                analyzeFavoriteAthletes();
                analyzeFavoriteTeams();
            }

            if (likes != null)
            {
                analyzeMyLikes();
                analyzeFriendsLikesFrequency();
            }

            if (checkins != null)
            {
                analyzeMyCheckins();
                analyzeFriendsCheckinFrequency();
            }

            prepareStatistics();
            return statistics;
        }

        private void analyzeFriendsCheckinFrequency()
        {
            FacebookProfileModel friend;
            Like like;
            CheckinData place;
            int value;
            friendsCheckinsFrequency = new Dictionary<string, int>();
          
            for (int i = 0; i < checkins.UserFriends.Friends.Count; i++)
            {
                friend = checkins.UserFriends.Friends.ElementAt(i);

                if (friend.UserCheckins != null)
                {
                    for (int j = 0; j < friend.UserCheckins.Checkins.Count(); j++)
                    {
                        place = friend.UserCheckins.Checkins.ElementAt(j);
                        if (friendsCheckinsFrequency.ContainsKey(place.Checkins.Name))
                        {
                            value = friendsCheckinsFrequency[place.Checkins.Name];
                            friendsCheckinsFrequency[place.Checkins.Name] = value + 1;
                        }
                        else
                        {
                            friendsCheckinsFrequency.Add(place.Checkins.Name, 1);
                        }
                    }
                }
            }
        }

        private void analyzeFriendsLikesFrequency()
        {
            FacebookProfileModel friend;
            Like like;
            CheckinData place;
            int value;
            friendslikesCategoriesFrequency = new Dictionary<string, int>();
            friendslikesFrequency = new Dictionary<string, int>();

            for (int i = 0; i < likes.UserFriends.Friends.Count; i++)
            {
                friend = likes.UserFriends.Friends.ElementAt(i);

                if (friend.UserLikes != null)
                {
                    for (int j = 0; j < friend.UserLikes.Likes.Count; j++)
                    {
                        like = friend.UserLikes.Likes.ElementAt(j);
                        if (friendslikesCategoriesFrequency.ContainsKey(like.Category))
                        {
                            value = friendslikesCategoriesFrequency[like.Category];
                            friendslikesCategoriesFrequency[like.Category] = value + 1;
                        }
                        else
                        {
                            friendslikesCategoriesFrequency.Add(like.Category, 1);
                        }

                        checkIfSimilar(friend, like);

                        if (like.ListCategories != null)
                        {
                            CategoryList category;
                            for (int m = 0; m < like.ListCategories.Count; m++)
                            {
                                category = like.ListCategories.ElementAt(m);
                                if (friendslikesCategoriesFrequency.ContainsKey(category.Name))
                                {
                                    value = friendslikesCategoriesFrequency[category.Name];
                                    friendslikesCategoriesFrequency[category.Name] = value + 1;
                                }
                                else
                                {
                                    friendslikesCategoriesFrequency.Add(category.Name, 1);
                                }

                                checkIfSimilar(friend, category);
                            }
                        }

                        if (friendslikesFrequency.ContainsKey(like.Name))
                        {
                            value = friendslikesFrequency[like.Name];
                            friendslikesFrequency[like.Name] = value + 1;
                        }
                        else
                        {
                            friendslikesFrequency.Add(like.Name, 1);
                        }
                    }
                }
            }
        }

        private void analyzeMyLikes()
        {
            Like like;
            int value;
            myLikesCategoriesFrequency = new Dictionary<string, int>();
            myLikesFrequency = new Dictionary<string, int>();

            for (int i = 0; i < likes.UserLikes.Likes.Count; i++)
            {
                like = likes.UserLikes.Likes.ElementAt(i);
                if (myLikesCategoriesFrequency.ContainsKey(like.Category))
                {
                    value = myLikesCategoriesFrequency[like.Category];
                    myLikesCategoriesFrequency[like.Category] = value + 1;
                }
                else
                {
                    myLikesCategoriesFrequency.Add(like.Category, 1);
                }

                if (like.ListCategories != null)
                {
                    CategoryList category;
                    for (int m = 0; m < like.ListCategories.Count; m++)
                    {
                        category = like.ListCategories.ElementAt(m);
                        if (myLikesCategoriesFrequency.ContainsKey(category.Name))
                        {
                            value = myLikesCategoriesFrequency[category.Name];
                            myLikesCategoriesFrequency[category.Name] = value + 1;
                        }
                        else
                        {
                            myLikesCategoriesFrequency.Add(category.Name, 1);
                        }
                    }
                }

                if (myLikesFrequency.ContainsKey(like.Name))
                {
                    value = myLikesFrequency[like.Name];
                    myLikesFrequency[like.Name] = value + 1;
                }
                else
                {
                    myLikesFrequency.Add(like.Name, 1);
                }
            }
        }

        private void checkIfSimilar(FacebookProfileModel user, Like like)
        {
            int value;
            if (myLikesCategoriesFrequency.ContainsKey(like.Category))
            {
                if (similarFriends.ContainsKey(user))
                {
                    value = similarFriends[user];
                    similarFriends[user] = value + 1;
                }
                else
                {
                    similarFriends.Add(user, 1);
                }
            }
        }

        private void checkIfSimilar(FacebookProfileModel user, Place place)
        {
            int value;
            if (myCheckins.ContainsKey(place.Name))
            {
                if (similarFriends.ContainsKey(user))
                {
                    value = similarFriends[user];
                    similarFriends[user] = value + 1;
                }
                else
                {
                    similarFriends.Add(user, 1);
                }
            }
        }

        private void checkIfSimilar(FacebookProfileModel user, CategoryList category)
        {
            int value;
            if (myLikesCategoriesFrequency.ContainsKey(category.Name))
            {
                if (similarFriends.ContainsKey(user))
                {
                    value = similarFriends[user];
                    similarFriends[user] = value + 1;
                }
                else
                {
                    similarFriends.Add(user, 1);
                }
            }
        }

        private void analyzeMusic()
        {
            if (basicInfo.UserMusic != null)
            {
                if (basicInfo.UserMusic.Music.Count >= Config.LIKES_MOVIES_LIMIT)
                {
                    likesMusic = (int)CommonModels.Status.True;
                }
                else
                {
                    likesMusic = (int)CommonModels.Status.False;
                }
            }
            else
            {
                likesMusic = (int)CommonModels.Status.Unknown;
            }
        }

        private void analyzeBooks()
        {
            if (basicInfo.UserBooks != null)
            {
                if (basicInfo.UserBooks.BookData.Count >= Config.LIKES_BOOK_LIMIT)
                {
                    likesBooks = (int)CommonModels.Status.True;
                }
                else
                {
                    likesBooks = (int)CommonModels.Status.True;
                }
            }
            else
            {
                likesBooks = (int)CommonModels.Status.Unknown;
            }
        }

        private void analyzeVideos()
        {
            if (basicInfo.UserVideos != null)
            {
                if (basicInfo.UserVideos.VideosData.Count >= Config.LIKES_MOVIES_LIMIT)
                {
                    likesMovies = (int)CommonModels.Status.True;
                }
                else
                {
                    likesMovies = (int)CommonModels.Status.True;
                }
            }
            else
            {
                likesMovies = (int)CommonModels.Status.Unknown;
            }
        }

        private void analyzeSports()
        {
            if (basicInfo.Sports != null)
            {
                isSportsman = (int)CommonModels.Status.True;
                likesSports = (int)CommonModels.Status.True;
            }
            else
            {
                isSportsman = (int)CommonModels.Status.False;
            }
        }

        private void analyzeFavoriteAthletes()
        {
            if (basicInfo.FavoriteAthletes != null)
            {
                if (basicInfo.FavoriteAthletes.Count >= Config.LIKES_SPORTS)
                {
                    likesSports = (int)CommonModels.Status.True;
                }
                else if (likesSports != (int)CommonModels.Status.True)
                {
                    likesSports = (int)CommonModels.Status.False;
                }

            }
            else
            {
                likesSports = (int)CommonModels.Status.Unknown;
            }
        }

        private void analyzeFavoriteTeams()
        {
            if (basicInfo.FavoriteTeams != null)
            {
                if (basicInfo.FavoriteTeams.Count >= Config.LIKES_SPORTS)
                {
                    likesSports = (int)CommonModels.Status.True;
                }
                else if (likesSports != (int)CommonModels.Status.True)
                {
                    likesSports = (int)CommonModels.Status.False;
                }

            }
            else
            {
                likesSports = (int)CommonModels.Status.Unknown;
            }
        }


        private void analyzeMyCheckins()
        {
            myCheckins = new Dictionary<string, int>();
            CheckinData place;
            int value;

            if (checkins.UserCheckins != null)
            {
                for (int i = 0; i < checkins.UserCheckins.Checkins.Count(); i++)
                {
                    place = checkins.UserCheckins.Checkins.ElementAt(i);
                    if (myCheckins.ContainsKey(place.Checkins.Name))
                    {
                        value = myCheckins[place.Checkins.Name];
                        myCheckins[place.Checkins.Name] = value + 1;
                    }
                    else
                    {
                        myCheckins.Add(place.Checkins.Name, 1);
                    }
                }
            }
        }

        private void prepareStatistics()
        {
            statistics = new FacebookStatistics();
            statistics.FriendsLikesCategoriesFrequency = friendslikesCategoriesFrequency;
            statistics.FriendsLikesFrequency = friendslikesFrequency;
            statistics.UserLikesCategoriesFrequency = myLikesCategoriesFrequency;
            statistics.UserLikesFrequency = myLikesFrequency;
            statistics.similarFriends = similarFriends;
            statistics.UserCheckinsFrequency = myCheckins;
            statistics.FriendsCheckinsFrequency = friendsCheckinsFrequency;

            if (friendslikesCategoriesFrequency != null)
            {
                statistics.SortedFriendsLikesCategoriesFrequency = Utilities.returnSortedKeyValuePair(friendslikesCategoriesFrequency);
            }
            if (friendslikesFrequency != null)
            {
                statistics.SortedFriendsLikesFrequency = Utilities.returnSortedKeyValuePair(friendslikesFrequency);
            }
            if (myLikesCategoriesFrequency != null)
            {
                statistics.SortedUserLikesCategoriesFrequency = Utilities.returnSortedKeyValuePair(myLikesCategoriesFrequency);
            }
            if (myLikesFrequency != null)
            {
                statistics.SortedUserLikesFrequency = Utilities.returnSortedKeyValuePair(myLikesFrequency);
            }
            if (similarFriends != null)
            {
                statistics.SortedSimillarFriends = Utilities.returnSortedKeyValuePair(similarFriends);
            }
            if (friendsCheckinsFrequency != null)
            {
                statistics.SortedFriendsCheckinsFrequency = Utilities.returnSortedKeyValuePair(friendsCheckinsFrequency);
            }
            if (myCheckins != null)
            {
                statistics.SortedUserCheckinsFrequency = Utilities.returnSortedKeyValuePair(myCheckins);
            }
           
            statistics.likesMusic = likesMusic;
            statistics.likesBooks = likesBooks;
            statistics.likesMovies = likesMovies;
            statistics.isSportsman = isSportsman;
            statistics.likesSports = likesSports;

            //statistics.CountFriendsAnalzyed = basicInfo.UserFriends.Friends.Count();
            //statistics.CountFriendsLikes = statistics.FriendsLikesFrequency.Count();
            //statistics.CountUserLikes = statistics.UserLikesFrequency.Count();
            statistics.Name = basicInfo.Name;
        }
    }
}