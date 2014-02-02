using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using smartGPS.Business.Custom;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business
{
    public class NotificationsManager
    {

        public static List<Notifications> getAllActiveNotifications()
        {
            return NotificationsDao.getAllActiveNotification().ToList<Notifications>();
        }

        public static Notifications getById(String id)
        {
            return NotificationsDao.getById(id);
        }

        public static void deactivateNotification(String id)
        {
            Notifications notification = getById(id);
            NotificationsDao.deactivateNotification(notification);
        }

        public static List<NotificationCategory> getNotificationCategories()
        {
            return NotificationsDao.getAllNotificationCateogries().ToList<NotificationCategory>();
        }

        public static String addNew(String text, String category, double latitude, double longitude, String userId, String address)
        {
            String id = Guid.NewGuid().ToString();
            NotificationsDao.addNew(id, DateTime.Now, text, category, latitude, longitude, userId, address);
            return id;
        }

        public static void sendToAllUsers(String text)
        {
            List<String> gcmIds = getAllGcmIds();
            List<String> responses = new List<string>();

            foreach (String id in gcmIds)
            {
                sendNotification(id, text);
            }
        }

        public static List<String> getAllGcmIds()
        {
            IEnumerable<User> users = UserAdministration.getAll();

            List<String> gcmIds = new List<string>();

            foreach (User user in users)
            {
                gcmIds.Add(user.GcmId);
            }

            return gcmIds;
        }

        public static void sendNotification(string regId, string text)
        {
            var applicationID = Config.GOOGLE_SERVER_API ;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"registration_ids\":[\"" + regId + "\"]," +
                            "\"data\": {\"title\": \"" + text + "\"}}";
                Console.WriteLine(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }

        public static void sendNotification(string regId, Notifications notification)
        {
            string jsonObject = "{\"user\": \"" + notification.User.Username + "\", \"latitude\": \"" + notification.Latitude + "\", \"longitude\": \"" + notification.Longitude + "\", \"text\":\"" + notification.Text + "\", \"category\":\"" + notification.CategoryId + "\"}";
            var applicationID = Config.GOOGLE_SERVER_API;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"registration_ids\":[\"" + regId + "\"]," +
                            "\"data\": " + jsonObject + "}";
                Console.WriteLine(json);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }

        public static void notifyUsersNearNotification(String notificationId, String userId)
        {
            Notifications notification = getById(notificationId);
            List<User> users = TravelManager.getAllUsersNearLocation(notification.Latitude, notification.Longitude, Config.NOTIFICATION_RADIUS, userId);

            foreach (User item in users)
            {
                sendNotification(item.GcmId, notification);
            }
        }

        public static List<Notifications> getNotificationsNearLocation(double latitude, double longitude)
        {
            List<Notifications> notifications = getAllActiveNotifications();
            List<Notifications> nearNotifications = new List<Notifications>();
            SmartLocation location = new SmartLocation();
            SmartLocation notificationLocation = new SmartLocation();
            location.Latitude = latitude;
            location.Longitude = longitude;
            Haversine haversine = new Haversine();
            double distance;

            foreach (var item in notifications)
            {
                notificationLocation.Latitude = item.Latitude;
                notificationLocation.Longitude = item.Longitude;
                distance = haversine.Distance(location, notificationLocation, Haversine.DistanceType.Kilometers);

                if (distance <= Config.NOTIFICATION_RADIUS)
                    nearNotifications.Add(item);
            }

            return nearNotifications;
        }

        public static void thumbsUp(String notificationId)
        {
            Notifications notification = getById(notificationId);
            NotificationsDao.thumbsUp(notification);
        }

        public static void thumbsDown(String notificationId)
        {
            Notifications notification = getById(notificationId);
            NotificationsDao.thumbsDown(notification);

            checkIfNotificationHasToBeDeactivated(notification);
        }

        public static Boolean checkIfNotificationHasToBeDeactivated(Notifications notification)
        {
            long difference = notification.ThumbsDown - notification.ThumbsUp;

            if(difference > Config.NOTIFICATION_CONSENSUS)
            {
                deactivateNotification(notification.Id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}