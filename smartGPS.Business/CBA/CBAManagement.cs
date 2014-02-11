using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using smartGPS.Business.CBA.Models;
using smartGPS.Persistance;

namespace smartGPS.Business.CBA
{
    public class CBAManagement
    {

        public static String CondSet_AddCondSet(CondSet condSet)
        {
            String id = Guid.NewGuid().ToString();
            String conditions = JsonConvert.SerializeObject(condSet.Conditions, Formatting.Indented);
            smartGPS.Persistance.CBADao.CondSet_Add(id, condSet.CondSupCount, conditions);

            return id;
        }

        public static void RuleItem_AddRuleItem(RuleItem ruleItem)
        {
            String id = Guid.NewGuid().ToString();
            String condSetId = CondSet_AddCondSet(ruleItem.Conditions);

            smartGPS.Persistance.CBADao.RuleItem_Add(id, ruleItem.UserId, ruleItem.RuleSupCount, ruleItem.ClassKey, ruleItem.ClassValue, ruleItem.DataSize, condSetId);
        }

        public static void RuleItem_DeleteAllForUser(String userId)
        {
            smartGPS.Persistance.CBADao.RuleItem_GetAllForUser(userId);
        }

        public static List<RuleItem> RuleItem_GetForUser(String userId)
        {
            IEnumerable<RuleItemTable> dbRules = CBADao.RuleItem_GetAllForUser(userId);

            List<RuleItem> rules = new List<RuleItem>();

            foreach (RuleItemTable rule in dbRules)
            {
                RuleItem ruleItem = new RuleItem();
                CondSet conditions = new CondSet();

                conditions.CondSupCount = rule.CondSetTable.CondSupCount;
                conditions.Conditions = JsonConvert.DeserializeObject<Dictionary<String, String>>(rule.CondSetTable.CondtionsJson);

                ruleItem.UserId = rule.UserId;
                ruleItem.RuleSupCount = rule.RuleSupCount;
                ruleItem.DataSize = rule.DataSize;
                ruleItem.ClassKey = rule.ClassKey;
                ruleItem.ClassValue = rule.ClassValue;
                ruleItem.Conditions = conditions;

                rules.Add(ruleItem);
            }

            return rules;
        }
    }
}