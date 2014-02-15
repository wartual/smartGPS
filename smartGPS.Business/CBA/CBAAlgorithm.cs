using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accord.Statistics.Analysis;
using smartGPS.Business.CBA.Models;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business.CBA
{
    public class CBAAlgorithm
    {
        private CBA_RG ruleGenerator{get; set;}
        private CBA_CB classificatorBuilder { get; set; }
        private String userId { get; set; }
        private double support { get; set; }
        private double confidence { get; set; }
        private int[] predicted { get; set; }
        private int[] expected { get; set; }

        public CBAAlgorithm(String userId)
        {
            this.userId = userId;
            this.support = Config.CBA_DEFAULT_SUPPORT;
            this.confidence = Config.CBA_DEFAULT_CONFIDENCE;
        }

        public CBAAlgorithm(String userId, double support, double confidence)
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

        public String classify(FacebookProccesedEntries entry)
        {
            return classificatorBuilder.classify(entry); 
        }

        public ConfusionMatrix test(int classValue)
        {
            String predictedValue;
            predicted = new int[199];
            expected = new int[199];

            List<RuleItem> ruleItems = generateRules();
            classificatorBuilder = new CBA_CB(ruleItems, userId);
            classificatorBuilder.generateClassificator();

            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            int i = 0;
            int notClassified = 0;
            foreach (FacebookProccesedEntries entry in entries)
            {
                predictedValue = classificatorBuilder.classify(entry);

                if (predictedValue.Equals(""))
                {
                    predicted[i] = 0;
                    notClassified++;
                }
                else
                {
                    if (Int16.Parse(predictedValue) == classValue)
                    {
                        predicted[i] = 1;
                    }
                    else
                    {
                        predicted[i] = 0;
                    }
                }

                if (entry.Category == classValue)
                {
                    expected[i] = 1;
                }
                else
                {
                    expected[i] = 0;
                }
                i++;
            }

            ConfusionMatrix matrix = new ConfusionMatrix(predicted, expected, 1, 0);
            return matrix;
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