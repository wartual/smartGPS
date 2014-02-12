using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using smartGPS.Business.CBA.Models;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business.CBA
{
    public class CBA_CB
    {

        private List<RuleItem> ruleSet { get; set; }
        private double support { get; set; }
        private double confidence { get; set; }
        private String userId { get; set; }
        private String tempKey { get; set; }
        private String tempValue { get; set; }
        private Object LinqObject = new FacebookProccesedEntries();

        public CBA_CB(List<RuleItem> ruleSet, String userId)
        {
            this.ruleSet = ruleSet;
            this.support = Config.CBA_DEFAULT_SUPPORT;
            this.confidence = Config.CBA_DEFAULT_CONFIDENCE;
            this.userId = userId;
        }

        public CBA_CB(List<RuleItem> ruleSet, double support, double confidence, String userId)
        {
            this.ruleSet = ruleSet;
            this.support = support;
            this.confidence = confidence;
            this.userId = userId;
        }

        public String classify(FacebookProccesedEntries entry)
        {
            String targetClassValue = "";
            String tempKey = "";
            String tempValue = "";
            Boolean tempDataSet;
            Boolean condSetFound = true;

            //put rules in array from database. This only happens when classifying first edge in path. Increases speed
            ruleSet = new List<RuleItem>();
            List<RuleItem> query = CBAManagement.RuleItem_GetForUser(userId);
            foreach (RuleItem x in query)
            {
                ruleSet.Add(x);
            }

            //pass over all rules in database stored in array list
            foreach (RuleItem r in ruleSet)
            {
                //pass over all conditions of each rule
                foreach (KeyValuePair<String, String> c in r.Conditions.Conditions)
                {
                    int i;

                    /*
                     * pass over all data in target case. This parts finds if rule condition set subset of
                     * given case conditions.
                     */
                    int count = 0;
                    foreach (PropertyInfo pi in LinqObject.GetType().GetProperties())
                    {
                        tempDataSet = getTempKeyAndValue(pi.Name, entry);
                        if (!tempDataSet)
                            continue;
                        else
                            count++;

                        if (this.tempKey.Equals(c.Key) && this.tempValue.Equals(c.Value))
                        {
                            break;
                        }
                    }

                    /// 6 is the number of colums in FacebookProccessedEntries which are important
                    if (count == Config.FB_PROCCESSED_ENTRIES_ATTRIBUTE_COLUMNS)
                    {
                        condSetFound = false;
                        break;
                    }
                }
                //if subset is found rule covers data case
                if (condSetFound)
                {
                    targetClassValue = r.ClassValue;
                    if (r.Conditions.Conditions.Count == 0)
                    {
                        targetClassValue = "0.0";
                    }
                    break;
                }
                condSetFound = true;
            }

            return targetClassValue;
        }

        /// <summary>
        ///  If classificator is built successfully returns 0, else it returns 1.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void generateClassificator()
        {
            List<RuleItem> rulesC = new List<RuleItem>();
            int returnValue = 0;
            List<String> d;
            List<String> dataSetIds = new List<String>();
            Boolean marked;
            String tempKey = "";
            String tempValue = "";
            String defaultClass = "";
            String tempClassValue = "";
            Boolean condSetFound = true;
            RuleItem r;
            int minErrorIndex = 0;
            int minError = Int16.MaxValue;
            int tempError = 0;
            int correct = 0;
            int defaultCT = 0;
            Boolean tempDataSet;
            int defaultCF = 0;
            ruleSet = sortR(ruleSet);

            CBAManagement.RuleItem_DeleteAllForUser(userId);
            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);

            //store ProcessedEntry ids from database
            foreach (FacebookProccesedEntries entry in entries)
            {
                dataSetIds.Add(entry.Id);
            }

            //if there are rules found
            if (ruleSet.Any())
            {
                //pass over all rules generated in rule generation part of CBA
                foreach (RuleItem x in ruleSet)
                {
                    d = new List<String>();
                    marked = false;

                    //pass over all entries in database
                    foreach (FacebookProccesedEntries entry in entries)
                    {
                        if (!dataSetIds.Contains(entry.Id))
                        {
                            continue;
                        }

                        //find if rule condition set is subset of case
                        foreach (KeyValuePair<String, String> c in x.Conditions.Conditions)
                        {
                            int i;
                            int count = 0;
                            foreach (PropertyInfo pi in LinqObject.GetType().GetProperties())
                            {
                                tempDataSet = getTempKeyAndValue(pi.Name, entry);
                                if (!tempDataSet)
                                    continue;
                                else
                                    count++;
                                tempClassValue = entry.Category.ToString();

                                if (this.tempKey.Equals(c.Key) && this.tempValue.Equals(c.Value))
                                {
                                    break;
                                }
                            }
                            if (count == Config.FB_PROCCESSED_ENTRIES_ATTRIBUTE_COLUMNS)
                            {
                                condSetFound = false;
                                break;
                            }
                        }

                        //if rule covers case than check if classification is correct. If true mark rule
                        if (condSetFound)
                        {
                            d.Add(entry.Id);
                            if (x.ClassValue.Equals(entry.Category.ToString()))
                                marked = true;
                        }

                        condSetFound = true;
                    }

                    if (marked)
                    {
                        //insert rule at the end of classifier
                        rulesC.Add(x);

                        //remove rule id from rule ids so it will not be used in future
                        foreach (String s in d)
                        {
                            dataSetIds.Remove(s);
                        }

                        //call method that select default class
                        defaultClass = selectDefaultClass(dataSetIds, rulesC);

                        //pass over all entries in database
                        foreach (FacebookProccesedEntries entry in entries)
                        {
                            //pass over all rules
                            int j;
                            for (j = 0; j < rulesC.Count; j++)
                            {
                                r = rulesC.ElementAt(j);
                                //check if rule covers entry case
                                foreach (KeyValuePair<String, String> c in r.Conditions.Conditions)
                                {
                                    int i;
                                    int count = 0;
                                    foreach (PropertyInfo pi in LinqObject.GetType().GetProperties())
                                    {
                                        tempDataSet = getTempKeyAndValue(pi.Name, entry);
                                        if (!tempDataSet)
                                            continue;
                                        else
                                            count++;

                                        if (this.tempKey.Equals(c.Key) && this.tempValue.Equals(c.Value))
                                        {
                                            break;
                                        }
                                    }
                                    if (count == Config.FB_PROCCESSED_ENTRIES_ATTRIBUTE_COLUMNS)
                                    {
                                        condSetFound = false;
                                        break;
                                    }
                                }

                                //if rule covers entry case, check if it is classified correctly
                                if (condSetFound)
                                {
                                    if (tempClassValue.Equals(r.ClassValue))
                                    {
                                        correct++;
                                    }
                                    else
                                    {
                                        tempError++;
                                    }
                                    break;
                                }
                                condSetFound = true;
                            }

                            //if case is not covered, try default class
                            if (j == rulesC.Count)
                            {
                                if (tempClassValue.Equals(defaultClass))
                                {
                                    defaultCT++;
                                }
                                else
                                {
                                    defaultCF++;
                                }
                            }
                        }

                        //get index in classificator rule set at which number of errors is minimal. Prune at found index
                        if (minError > (tempError + defaultCF))
                        {
                            minErrorIndex = rulesC.Count;
                            minError = tempError + defaultCF;
                        }
                        tempError = 0;
                        correct = 0;
                        defaultCT = 0;
                        defaultCF = 0;
                    }
                }
            }
            else
            {
                defaultClass = selectDefaultClass(dataSetIds, rulesC);
            }

            for (int i = 0; i < minErrorIndex; i++)
            {
                CBAManagement.RuleItem_AddRuleItem(rulesC.ElementAt(i));
            }

            ////add default class: {} --> defaultClassValue
            //RuleItem def = new RuleItem();
            //def.ClassKey = "Category";
            //def.ClassValue = selectDefaultClass(dataSetIds, rulesC);
            //def.Conditions = new CondSet();
            //def.UserId = userId;

            //CBAManagement.RuleItem_AddRuleItem(def);
        }

        /// <summary>
        ///     Default class is the most common class that is left in the remaining data cases
        /// </summary>
        /// <param name="dataSetIds"></param>
        /// <param name="rulesC"></param>
        /// <returns></returns>
        private String selectDefaultClass(List<String> dataSetIds, List<RuleItem> rulesC)
        {
            int maxCount = 0;
            int value;
            String defaultClass = null;
            Dictionary<String, int> classes = new Dictionary<String, int>();
            FacebookProccesedEntries entry;
            List<FacebookProccesedEntries> entriesForUser = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);

            foreach (String id in dataSetIds)
            {
                entry = entriesForUser.Where(item => item.Id.Equals(id)).Single();


                if (classes.ContainsKey(entry.Category.ToString()))
                {
                    value = classes[entry.Category.ToString()] + 1;
                    classes[entry.Category.ToString()] = value;
                }
                else
                {
                    classes.Add(entry.Category.ToString(), 1);
                }
            }

            foreach (KeyValuePair<String, int> c in classes)
            {
                if (c.Value > maxCount)
                {
                    defaultClass = c.Key;
                    maxCount = c.Value;
                }
            }

            return defaultClass;
        }

        private List<RuleItem> sortR(List<RuleItem> ruleSet)
        {
            ruleSet.Sort();
            return ruleSet;
        }

        #region Utils
        private Boolean getTempKeyAndValue(String name, FacebookProccesedEntries entry)
        {
            if (name.Equals("Id"))
                return false;
            else if (name.Equals("Sportsman"))
            {
                tempKey = name;
                tempValue = entry.Sportsman;
                return true;
            }
            else if (name.Equals("LikesBook"))
            {
                tempKey = name;
                tempValue = entry.LikesBooks;
                return true;
            }
            else if (name.Equals("LikesMusic"))
            {
                tempKey = name;
                tempValue = entry.LikesMusic;
                return true;
            }
            else if (name.Equals("LikesSports"))
            {
                tempKey = name;
                tempValue = entry.LikesSports;
                return true;
            }
            else if (name.Equals("LikesTravelling"))
            {
                tempKey = name;
                tempValue = entry.LikesTravelling;
                return true;
            }
            else if (name.Equals("LikesMovies"))
            {
                tempKey = name;
                tempValue = entry.LikesMovies;
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}