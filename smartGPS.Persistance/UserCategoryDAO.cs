using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class UserCategoryDAO:BaseClass
    {

        public static void createNew(int id, String category)
        {
            UserCategory model = new UserCategory();
            model.Id = id;
            model.Category = category;

            db.UserCategory.Add(model);
            db.SaveChanges();
        }

        public static void createNew(UserCategory userCategory)
        {
            db.UserCategory.Add(userCategory);
            db.SaveChanges();
        }

        public static IEnumerable<UserCategory> getAll()
        {
            return db.UserCategory;
        }

        public static UserCategory getById(int id)
        {
            return db.UserCategory.Where(item => item.Id == id).SingleOrDefault();
        }
    }
}