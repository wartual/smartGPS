using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class GooglePlacesAPIDao:BaseClass
    {

        public static IEnumerable<GooglePlacesAPICategories> Categories_getAll()
        {
            return db.GooglePlacesAPICategories;
        }

        public static IEnumerable<GooglePlacesAPICategories> Categories_getAllByCategory(int categoryId)
        {
            return db.GooglePlacesAPICategories.Where(item => item.UserCategoryId == categoryId);
        }

        public static GooglePlacesAPICategories Categories_getById(int id)
        {
            return db.GooglePlacesAPICategories.Where(item => item.Id == id).SingleOrDefault();
        }
    }
}