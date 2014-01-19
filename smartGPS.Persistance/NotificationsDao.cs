using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class NotificationsDao:BaseClass
    {

        public static Notifications getById(String id)
        {
            return db.Notifications.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static IEnumerable<NotificationCategory> getAllNotificationCateogries()
        {
            return db.NotificationCategory;
        }

        public static void addNew(String id, DateTime date, String text, String category, double latitude, double longitude, String userId, String address)
        {
            Notifications notification = new Notifications();
            notification.Active = true;
            notification.CategoryId = category;
            notification.DateCreated = date;
            notification.DateUpdated = date;
            notification.Id = id;
            notification.Latitude = latitude;
            notification.Longitude = longitude;
            notification.Text = text;
            notification.UserId = userId;
            notification.Address = address;

            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        public static IEnumerable<Notifications> getAllActiveNotification()
        {
            return db.Notifications.Where(item => item.Active == true);
        }

        public static void deactivateNotification(Notifications notification)
        {
            notification.Active = false;
            notification.DateUpdated = DateTime.Now;

            db.SaveChanges();
        }
    }
}