using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class FoursquareDao:BaseClass
    {
        public static FoursquareProfile getProfileByUserId(String userId)
        {
            return db.FoursquareProfile.Where(item => item.UserId.Equals(userId)).SingleOrDefault();
        }

        public static void addNewProfile(String userId, String userCheckins, String recent)
        {
            FoursquareProfile model = new FoursquareProfile();
            model.UserId = userId;
            model.UserCheckins = userCheckins;
            model.Recent = recent;
            model.DateCreated = DateTime.Now;
            model.DateUpdated = DateTime.Now;
            model.Id = Guid.NewGuid().ToString();

            db.FoursquareProfile.Add(model);
            db.SaveChanges();
        }

        public static void addNewProfile(FoursquareProfile profile)
        {
            db.FoursquareProfile.Add(profile);
            db.SaveChanges();
        }

        public static void updateProfile(String userId, String userCheckins, String recent)
        {
            FoursquareProfile profile = getProfileByUserId(userId);

            if (userCheckins != null)
            {
                profile.UserCheckins = userCheckins;
            }

            if (recent != null)
            {
                profile.Recent = recent;
            }

            profile.DateUpdated = DateTime.Now;
            db.SaveChanges();
        }

        public static IEnumerable<FoursquareVenuesCategories> Categories_getAll()
        {
            return db.FoursquareVenuesCategories;
        }

        public static IEnumerable<FoursquareVenuesCategories> Categories_getAllByCategory(int categoryId)
        {
            return db.FoursquareVenuesCategories.Where(item => item.UserCategoryId == categoryId);
        }

        public static FoursquareVenuesCategories Categories_getById(String id)
        {
            return db.FoursquareVenuesCategories.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static void Categories_addNew(String id, String category, int? categoryId, String parentId)
        {
            FoursquareVenuesCategories model = new FoursquareVenuesCategories();
            model.Id = id;

            if(categoryId.HasValue)
                 model.UserCategoryId = categoryId.Value;

            if(parentId != null)
                model.Parent = parentId;

            model.Category = category;
            db.FoursquareVenuesCategories.Add(model);
            db.SaveChanges();
        }

    }
}