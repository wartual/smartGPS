using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartGPS.Business.CBA.Models
{
    public class RuleItem
    {
        //condition set
        private CondSet Conditions { get; set; }
        //rule support
        private int RuleSupCount { get; set; }
        //class key (speed difference or consumption)
        private String ClassKey { get; set; }
        //class value
        private String ClassValue { get; set; }
        //number of entries 
        private int DataSize { get; set; }
        //username
        private String UserId { get; set; }

        /**
	     * Constructor
	    */
        public RuleItem()
        {
            this.Conditions = new CondSet();
            this.RuleSupCount = 0;
            this.ClassKey = "";
            this.ClassValue = "";
            this.DataSize = 0;
            this.UserId = "";
        }


        /// <summary>Constructor that sets provided attributes</summary> 
        public RuleItem(String key, String value, CondSet cond, int dataSize, String user)
        {
            this.Conditions = cond;
            this.RuleSupCount = 1;
            this.ClassKey = key;
            this.ClassValue = value;
            this.DataSize = dataSize;
            this.UserId = user;
        }


        ///<summary>Merges this and attribute rule items into one. k is size of condition set
	    ///and is used to check if two conditions sets can be merged based on first k-2 entries
	    ///which must be the same</summary>
        public RuleItem Merge(RuleItem rule, int k)
        {
            RuleItem newRule = new RuleItem();

            //1) preveri isti class
            //2) nastavi class
            //3) merge conditions
            //4) return rule

            if (ClassKey.Equals(rule.ClassKey) && ClassValue.Equals(rule.ClassValue))
            {
                newRule.ClassKey = ClassKey;
                newRule.ClassValue = ClassValue;
                newRule.DataSize = DataSize;
                newRule.UserId = UserId;

                CondSet newCondSet = Conditions.merge(rule.Conditions, k);
                if (newCondSet != null)
                {
                    newRule.Conditions = newCondSet;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            return newRule;
        }

    }
}