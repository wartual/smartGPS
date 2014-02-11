using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using smartGPS.Business.CBA.Models;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business.CBA
{
    public class CBA
    {
        private CBA_RG ruleGenerator{get; set;}
        private CBA_CB classificatorBuilder { get; set; }
        private String userId { get; set; }
        private double support { get; set; }
        private double confidence { get; set; }

        public CBA(String userId)
        {
            this.userId = userId;
            this.support = Config.CBA_DEFAULT_SUPPORT;
            this.confidence = Config.CBA_DEFAULT_CONFIDENCE;
        }

        public CBA(String userId, double support, double confidence)
        {
            this.userId = userId;
            this.support = support;
            this.confidence = confidence;
        }

        public List<RuleItem> generateRules()
        {
            ruleGenerator = new CBA_RG(support, confidence, userId);
            return ruleGenerator.getRulesSet();
        }

        public String performClassification(FacebookProccesedEntries entry)
        {
            List<RuleItem> ruleItems = ruleGenerator.getRulesSet();
            classificatorBuilder = new CBA_CB(ruleItems, userId);
            classificatorBuilder.generateClassificator();

            return classificatorBuilder.classify(entry);
            //return "xx";
        }

        public String performClassification()
        {
            List<RuleItem> ruleItems = generateRules();
            classificatorBuilder = new CBA_CB(ruleItems, userId);
            classificatorBuilder.generateClassificator();
            Dictionary<FacebookProccesedEntries, String> test = new Dictionary<FacebookProccesedEntries, string>();
            String classValue;
            int categorized = 0;
            int trueCategorized = 0;

            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);

            foreach (FacebookProccesedEntries entry in entries)
            {
                classValue = classificatorBuilder.classify(entry);

                if(!classValue.Equals(""))
                {
                    categorized++;
                }

                if(classValue.Equals(entry.Category.ToString()))
                {
                    trueCategorized++;
                }

                test.Add(entry, classValue);
            }

            return "";
        }

        public void generateClassificator(List<RuleItem> ruleItems)
        {
            classificatorBuilder = new CBA_CB(ruleItems, userId);
            classificatorBuilder.generateClassificator();
        }

        private FacebookProccesedEntries dummyModel()
        {
            FacebookProccesedEntries model = new FacebookProccesedEntries();
            model.UserId = userId;
            model.UserName = "Kristina Krznarić";
            model.LikesBooks = "Unknown";
            model.LikesMovies = "True";
            model.LikesMusic = "True";
            model.LikesSports = "True";
            model.LikesTravelling = "True";
            model.Sportsman = "False";
            return model;
        }
    }
}