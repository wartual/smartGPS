using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Persistance
{
    public class CBADao:BaseClass
    {

        #region CondSet

        public static void CondSet_Add(CondSetTable condSet)
        {
            db.CondSetTable.Add(condSet);
            db.SaveChanges();
        }

        public static void CondSet_Add(String id, int condSupCount, String conditions)
        {
            CondSetTable model = new CondSetTable();
            model.Id = id;
            model.CondSupCount = condSupCount;
            model.CondtionsJson = conditions;
            model.DateCreated = DateTime.Now;

            db.CondSetTable.Add(model);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }

        public static CondSetTable CondSet_GetById(String id)
        {
            return db.CondSetTable.Where(item => item.Id.Equals(id)).SingleOrDefault();
        }

        public static Boolean CondSet_DeleteById(String id)
        {
            CondSetTable model = CondSet_GetById(id);
            if (model == null)
                return false;
            else
            {
                db.CondSetTable.Remove(model);
                db.SaveChanges();
                return true;
            }
        }

        #endregion

        #region RuleItem

        public static void RuleItem_Add(RuleItemTable item)
        {
            db.RuleItemTable.Add(item);
            db.SaveChanges();
        }

        public static void RuleItem_Add(String id, String userId, int ruleSuportCount, String classKey, String classValue, int dataSetSize, String condSetId)
        {
            RuleItemTable ruleItem = new RuleItemTable();
            ruleItem.ClassKey = classKey;
            ruleItem.ClassValue = classValue;
            ruleItem.CondSetId = condSetId;
            ruleItem.DataSize = dataSetSize;
            ruleItem.DateCreated = DateTime.Now;
            ruleItem.Id = id;
            ruleItem.UserId = userId;

            db.RuleItemTable.Add(ruleItem);

            try
            {
                db.SaveChanges();
            }
            catch(Exception e)
            {

            }
        }

        public static IEnumerable<RuleItemTable > RuleItem_GetAllForUser(String userId)
        {
            return db.RuleItemTable.Where(item => item.UserId.Equals(userId));
        }

        public static Boolean RuleItem_DeleteForUser(String userId)
        {
            IEnumerable<RuleItemTable> rules = RuleItem_GetAllForUser(userId);
            if (rules == null)
            {
                return false;
            }
            else
            {
                foreach (RuleItemTable ruleItem in rules)
                {
                    db.RuleItemTable.Remove(ruleItem);
                    CondSet_DeleteById(ruleItem.Id);
                }
                db.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}