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
    public class CBA_RG
    {

        private double Support{get; set;}
        private double Confidence{get; set;}
        private String UserId{get; set;}
        private String tempKey{get; set;}
        private String tempValue{get; set;}
        private Object LinqObject = new FacebookProccesedEntries();
        private List<RuleItem> RuleSet { get; set; }
        private int DataSetSize { get; set; }

        ///<summary>
        ///Constructor, needs support and confidence
        ///</summary>
        public CBA_RG(double support, double confidence, String userId)
        {
            this.Support = support;
            this.Confidence = confidence;
            this.UserId = userId;
        }

        public CBA_RG()
        {
            this.Support = Config.CBA_DEFAULT_SUPPORT;
            this.Support = Config.CBA_DEFAULT_CONFIDENCE;
        }

        public List<RuleItem> getRulesSet()
        {
            RuleSet = new List<RuleItem>();
            List<RuleItem> frequentOneRuleItems = generateFrequentOneRuleItems();
            generateFrequentKRuleItems(frequentOneRuleItems);

            return RuleSet;
        }

        private List<RuleItem> generateFrequentOneRuleItems()
        {
            List<RuleItem> rules = new List<RuleItem>();
            Boolean condSetFound = false;
            Boolean ruleFound = false;
            Boolean tempDataSet;
            String classKey = "Category";
            String tempClassValue;
            int tempCount = 0;

            IEnumerable<FacebookProccesedEntries> proccessedEntries = FacebookManagement.FacebookProccessedEntries_GetAllForUser(UserId).Take(3);
            DataSetSize = proccessedEntries.Count();

            foreach (FacebookProccesedEntries entry in proccessedEntries)
            {

                foreach (PropertyInfo pi in LinqObject.GetType().GetProperties())
                {
                    tempDataSet = getTempKeyAndValue(pi.Name, entry);
                    tempClassValue = entry.Category.ToString();
                    if (!tempDataSet)
                        continue;

                    //pass over all found rules. If rule exists increase support, else add new 1-rule
                    foreach (RuleItem rule in rules)
                    {
                        if (rule.Conditions.Conditions.ContainsKey(tempKey) && rule.Conditions.Conditions[tempKey].Equals(tempValue) &&
                            rule.ClassKey.Equals(classKey) && rule.ClassValue.Equals(tempClassValue))
                        {
                            rule.RuleSupCount = rule.RuleSupCount + 1;
                            rule.Conditions.CondSupCount = rule.Conditions.CondSupCount + 1;
                            ruleFound = true;
                            condSetFound = true;
                        }
                        else if (rule.Conditions.Conditions.ContainsKey(tempKey) && rule.Conditions.Conditions[tempKey].Equals(tempValue))
                        {
                            rule.Conditions.CondSupCount = rule.Conditions.CondSupCount + 1;
                            condSetFound = true;
                            tempCount = rule.Conditions.CondSupCount;
                        }
                    }


                    //if rule is not found and condition set is found, get condition set support and increase it in both rules
                    if (!ruleFound && condSetFound)
                    {
                        rules.Add(new RuleItem(classKey, tempClassValue, new CondSet(tempKey, tempValue), DataSetSize, UserId));
                        rules.ElementAt(rules.Count - 1).Conditions.CondSupCount = tempCount;
                    }
                    else if (!ruleFound)
                    {
                        rules.Add(new RuleItem(classKey, tempClassValue, new CondSet(tempKey, tempValue), DataSetSize, UserId));
                    }

                    condSetFound = false;
                    ruleFound = false;
                }
            }

            //if two rules have same condition set use one with better support and remove other
            for (int i = 0; i < rules.Count; i++)
            {
                RuleItem first = rules.ElementAt(i);

                for (int j = i + 1; j < rules.Count; j++)
                {
                    RuleItem temp = rules.ElementAt(j);
                    if (Utilities.compareDictionaries(first.Conditions.Conditions, temp.Conditions.Conditions))
                    {
                        if (temp.RuleSupCount <= first.RuleSupCount)
                        {
                            rules.RemoveAt(j);
                            j--;
                        }
                        else
                        {
                            rules.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }
      
            //rule list pruning based on support and confidence
            for(int i = 0; i< rules.Count; i++)
            {
                RuleItem temp = rules.ElementAt(i);
                if ((double)temp.RuleSupCount / (double)DataSetSize < Support)
                {
                    rules.RemoveAt(i);
                }
                else if ((double)temp.RuleSupCount / (double)temp.Conditions.CondSupCount < Confidence)
                {
                    rules.RemoveAt(i);
                }
            }

            RuleItem item;
        
            //sort based on class value
            for (int i = 0; i < rules.Count; i++)
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (rules.ElementAt(i).ClassValue.CompareTo(rules.ElementAt(j).ClassValue) > 0)
                    {
                        item = rules.ElementAt(i);
                        rules[i] = rules.ElementAt(j);
                        rules[j] = item;
                    }
                }
            }

            return rules;
        }

        private void generateFrequentKRuleItems(List<RuleItem> frequentOneRuleItems)
        {
            //if no 1-rules are found return
            if (frequentOneRuleItems == null)
            {
                return;
            }

            int k = 2;
            RuleItem newRule = null;
            String tempKey = "";
            String tempValue = "";
            List<RuleItem> rulesK = new List<RuleItem>();
            List<RuleItem> rulesC;
            String tempClassValue = "";
            Boolean condSetFound = true;
            rulesK.AddRange(frequentOneRuleItems);
            String classKey = "Category";
            int tempCount = 0;
            Boolean tempDataSet;

            //continue until there are still rules left to extend
            while (rulesK.Count != 0)
            {
                rulesC = new List<RuleItem>();

                ///   merge every rule with every rule. Merge function of RuleItem class will determine if this is possible
                ///   candidate generation
                for (int i = 0; i < rulesK.Count; i++)
                {
                    for (int j = i + 1; j < rulesK.Count; j++)
                    {
                        newRule = rulesK.ElementAt(i).Merge(rulesK.ElementAt(j), k);
                        if (newRule != null)
                        {
                            rulesC.Add(newRule);
                        }
                    }
                }

                RuleSet.AddRange(rulesK);
                IEnumerable<FacebookProccesedEntries> proccessedEntries = FacebookManagement.FacebookProccessedEntries_GetAllForUser(UserId).Take(3);
                DataSetSize = proccessedEntries.Count();
                foreach (FacebookProccesedEntries entry in proccessedEntries)
                {
                    tempClassValue = entry.Category.ToString();
                    //second pass over all rules
                    for (int j = 0; j < rulesC.Count; j++)
                    {
                        RuleItem r = rulesC.ElementAt(j);

                        //third pass over all conditions in each rule
                        foreach (KeyValuePair<String, String> c in r.Conditions.Conditions)
                        {
                            //fourth pass over all atributes in each record
                            int i;
                            PropertyInfo pi;
                            for (i = 0; i < LinqObject.GetType().GetProperties().Count(); i++)
                            {
                                pi = LinqObject.GetType().GetProperties().ElementAt(i);
                                tempDataSet = getTempKeyAndValue(pi.Name, entry);
                                if (!tempDataSet)
                                    continue;
                                if (tempKey.Equals(c.Key) && tempValue.Equals(c.Value))
                                {
                                    break;
                                }
                            }

                            /// 5 is the number of colums in FacebookProccessedEntries which are important
                            if (i == 5)
                            {
                                condSetFound = false;
                                break;
                            }
                        }

                        //if rule is not found and condition set is found, get condition set support and increase it in both rules
                        if (tempClassValue.Equals(r.ClassValue) && condSetFound)
                        {
                            r.RuleSupCount = r.RuleSupCount + 1;
                            r.Conditions.CondSupCount = r.Conditions.CondSupCount + 1;
                        }
                        else if (condSetFound)
                        {
                            r.Conditions.CondSupCount = r.Conditions.CondSupCount + 1;
                        }
                        condSetFound = true;
                    }
                }

                //rule pruning based on support and confidence
                RuleItem temp;
                for (int i = 0; i < rulesC.Count; i++)
                {
                    temp = rulesC.ElementAt(i);
                    if ((double)temp.RuleSupCount / (double)DataSetSize < Support)
                    {
                        rulesC.RemoveAt(i);
                    }
                    else if ((double)temp.RuleSupCount / (double)temp.Conditions.CondSupCount < Confidence)
                    {
                        rulesC.RemoveAt(i);
                    }
                }
              
                //rulesK to rules list
                rulesK = rulesC;
                k++;
            }
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