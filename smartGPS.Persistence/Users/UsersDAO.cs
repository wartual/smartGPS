using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistence.Users
{
    public class UsersDAO
    {

        private static smartgpsEntities db = new smartgpsEntities();

        public static void addNew(String username, String password, String name, String surname)
        {
            users model = new users();
            model.Id = Guid.NewGuid().ToString();
            model.Username = username;
            model.Password = password;
            model.Name = name;
            model.Surname = surname;
            model.DateCreated = DateTime.Now;
            model.DateLastLogin = DateTime.Now;

            db.users.Add(model);
            db.SaveChanges();
        }

        public static users getById(String id)
        {
            return db.users.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static users getByUsernameAndPassword(String username, String password)
        {
            return db.users.Where(item => item.Username.Equals(username) && item.Password.Equals(password)).SingleOrDefault();
        }

        public static Boolean alreadyExists(String username) 
        {
            users model = db.users.Where(item => item.Username.Equals(username)).SingleOrDefault();
            if (model == null)
                return false;
            else
                return true;
        }

        public static void updateDateLastLogin(users model)
        {
            model.DateLastLogin = DateTime.Now;
            db.SaveChanges();
        }
     }
}