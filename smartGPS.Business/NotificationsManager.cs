using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business
{
    public class NotificationsManager
    {
        public static string sendNotification(string deviceId, string message)
        {
            string GoogleAppID = Config.GOOGLE_SERVER_API;
            var SENDER_ID = Config.APP_ID;
            var value = message;
            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
            Console.WriteLine(postData);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            String sResponseFromServer = tReader.ReadToEnd();


            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }

        public static List<NotificationCategory> getNotificationCategories()
        {
            return NotificationsDao.getAllNotificationCateogries().ToList<NotificationCategory>();
        }

        public static void addNew(String text, String category, double latitude, double longitude, String userId)
        {
            NotificationsDao.addNew(Guid.NewGuid().ToString(), DateTime.Now, text, category, latitude, longitude, userId);
        }

        public static List<String> sendToAllUsers(String text)
        {
            List<String> gcmIds = getAllGcmIds();
            List<String> responses = new List<string>();

            foreach (String id in gcmIds)
            {
                responses.Add(sendNotification(id, text));
            }

            return responses;
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
    }
}