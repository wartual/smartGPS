using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class NotificationsDao:BaseClass
    {

        public static IEnumerable<NotificationCategory> getAllNotificationCateogries()
        {
            return db.NotificationCategory;
        }

        public static void addNew(String id, DateTime date, String text, String category, double latitude, double longitude, String userId)
        {
            Notifications notification = new Notifications();
            notification.Active = true;
            notification.CategoryId = category;
            notification.DateCreated = date;
            notification.Id = id;
            notification.Latitude = latitude;
            notification.Longitude = longitude;
            notification.Text = text;
            notification.UserId = userId;

            db.Notifications.Add(notification);
            db.SaveChanges();
        }
    }
}