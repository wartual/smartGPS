using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.CBA.Models
{
    public class CondSet
    {
        //Dictionary of conditions
        public Dictionary<String, String> Conditions { get; set; }
        //Support count
        public int CondSupCount { get; set; }

        // Constructor
        public CondSet()
        {
            this.Conditions = new Dictionary<string, string>();
            this.CondSupCount = 0;
        }

        /// <summary>Constructor that adds first item into dictionary
        /// <param>key</param>
        /// <param>value</param>
        /// </summary> 
        public CondSet(String key, String value)
        {
            this.Conditions = new Dictionary<string, string>();
            this.Conditions.Add(key, value);
            this.CondSupCount = 1;
        }

        public CondSet merge(CondSet cond, int k)
        {
            CondSet newSet = new CondSet();
            SortedDictionary<String, String> sortedOriginal = new SortedDictionary<String, String>(Conditions);
            SortedDictionary<String, String> sortedNew = new SortedDictionary<String, String>(cond.Conditions);
            int kCounter = 0;
            int i = 0;
            KeyValuePair<String, String> first;
            KeyValuePair<String, String> second;

            while (i < sortedOriginal.Count && i < sortedNew.Count)
            {
                first = sortedOriginal.ElementAt(i);
                second = sortedNew.ElementAt(i);
                if (kCounter < k - 2)
                {
                    if (!first.Equals(second))
                    {
                        return null;
                    }
                    else
                    {
                        newSet.addCondition(first.Key, first.Value);
                    }
                }
                else
                {
                    if (first.Key.Equals(second.Key))
                    {
                        return null;
                    }
                    else
                    {
                        newSet.addCondition(first.Key, first.Value);
                        newSet.addCondition(second.Key, second.Value);
                    }
                }
                kCounter++;
                i++;
            }

            return newSet;
        }

        public void addCondition(String key, String value)
        {
            this.Conditions.Add(key, value);
        }
    }
}