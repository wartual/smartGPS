using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using smartGPS.Business.Custom;
using smartGPS.Business.ExternalServices;
using smartGPS.Business.Models;
using smartGPS.Business.Models.Facebook;

namespace smartGPS.Business
{
    public class Utilities
    {

        public static Boolean compareDictionaries(Dictionary<String, String> first, Dictionary<String, String> second)
        {
            return first.Keys.Count == second.Keys.Count && first.Keys.All(k => second.ContainsKey(k) && object.Equals(second[k], first[k]));
        }

        public static String encryptPassword(String password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static double? parseDouble(String value)
        {
            try
            {
                return Double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static readonly DateTime UnixEpochStart =  DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

        public static DateTime ToDateTimeFromEpoch(long epochTime)
        {
            DateTime result = UnixEpochStart.AddMilliseconds(epochTime);
            return result;
        }

        public static DateTime ToDateTimeFromEpochInSeconds(long epochTime)
        {
               DateTime result = UnixEpochStart.AddSeconds(epochTime);
               return result;
        }

        public static long ToEpochFromDateTime(DateTime date)
        {
            TimeSpan ts = date.ToUniversalTime() - UnixEpochStart;
            return (long)Math.Floor(ts.TotalMilliseconds);
        }

        public static double calculateDistance(double departureLatitude, double departureLongitude, double destinationLatitude, double destinationLongitude)
        {
            SmartLocation departure = new SmartLocation(departureLatitude, departureLongitude);
          
            SmartLocation destination = new SmartLocation(destinationLatitude, destinationLongitude);
      
            Haversine haversine = new Haversine();
            return haversine.Distance(departure, destination, Haversine.DistanceType.Kilometers);
        }

        public static double calculateDistance(SmartLocation departure, SmartLocation destination)
        {
            Haversine haversine = new Haversine();
            return haversine.Distance(departure, destination, Haversine.DistanceType.Kilometers);
        }

        public static List<KeyValuePair<String, int>> returnSortedKeyValuePair(Dictionary<String,int> dictionary)
        {
            List<KeyValuePair<String, int>> list = dictionary.ToList();
            list.Sort((firstPair,nextPair) =>
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
            );
            list.Reverse();
            return list;
        }

        public static List<KeyValuePair<String, double>> returnSortedKeyValuePair(Dictionary<String, double> dictionary)
        {
            List<KeyValuePair<String, double>> list = dictionary.ToList();
            list.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );
            list.Reverse();
            return list;
        }

        public static List<KeyValuePair<int, double>> returnSortedKeyValuePair(Dictionary<int, double> dictionary)
        {
            List<KeyValuePair<int, double>> list = dictionary.ToList();
            list.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );
            list.Reverse();
            return list;
        }

        public static List<KeyValuePair<FacebookProfileModel, int>> returnSortedKeyValuePair(Dictionary<FacebookProfileModel, int> dictionary)
        {
            List<KeyValuePair<FacebookProfileModel, int>> list = dictionary.ToList();
            list.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );
            list.Reverse();
            return list;
        }

        public static String mapStatusEnumToWord(int enumerator)
        {
            if (enumerator == (int)CommonModels.Status.True)
                return "True";
            else if (enumerator == (int)CommonModels.Status.False)
                return "False";
            else
                return "Unknown";
        }

        public static int mapWordToStatusEnum(String word)
        {
            if (word.Equals("True"))
                return 0;
            else if (word.Equals("Unknown"))
                return 1;
            else
                return 2;
        }

        public static int mapSportsmanWordToStatusEnum(String word)
        {
            if (word.Equals("True"))
                return 0;
            else
                return 1;
        }

        public static String mapEnumCategoryToWord(int enumerator)
        {
            if (enumerator == (int)CommonModels.UserCategories.Traveller)
                return "Traveller";
            else if (enumerator == (int)CommonModels.UserCategories.Sportsman)
                return "Sportsman";
            else
                return "Music";
        }

    }
}