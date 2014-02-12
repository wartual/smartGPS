using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Statistics.Filters;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;
using Accord.Math;
using Accord.Statistics.Analysis;
using System.Linq.Expressions;

namespace smartGPS.Business.DecisionTrees
{
    public class DecisionTreesAlgorithm
    {
        private String userId { get; set; }
        private int classCount { get; set; }
        private DecisionTree tree { get; set; }
        private Codification codebook { get; set; }
        private DataTable data { get; set; }
        private FacebookProccesedEntries element { get; set; }
        private int[] outputs;
        private int[] predicted;
        private Expression<Func<double[], int>> expression { get; set; }
        
        public DecisionTreesAlgorithm(String userId, int classCount, FacebookProccesedEntries element)
        {
            this.userId = userId;
            this.element = element;
            this.classCount = classCount;
        }

        public DecisionTreesAlgorithm(String userId, FacebookProccesedEntries element)
        {
            this.userId = userId;
            this.element = element;
            this.classCount = Config.DECISION_TREES_CLASS_COUNT;
        }

        private void prepareData()
        {
            data = new DataTable("Facebook proccessed entries");
            data.Columns.Add("Id");
            data.Columns.Add("Sportsman");
            data.Columns.Add("LikesBooks");
            data.Columns.Add("LikesMusic");
            data.Columns.Add("LikesSports");
            data.Columns.Add("LikesTravelling");
            data.Columns.Add("LikesMovies");
            data.Columns.Add("Category");

            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            int i = 0;
            foreach (FacebookProccesedEntries entry in entries)
            {
                data.Rows.Add(i.ToString(), entry.Sportsman, entry.LikesBooks, entry.LikesMusic, entry.LikesSports, entry.LikesTravelling, entry.LikesMovies, entry.Category);
                i++;
            }

            // Create a new codification codebook to 
            // convert strings into integer symbols
            codebook = new Codification(data);

            DecisionVariable[] attributes =
            {
                new DecisionVariable("Sportsman",   2), // 3 possible values (True, False, Unknown)
                new DecisionVariable("LikesBooks", 3), // 3 possible values (True, False, Unknown) 
                new DecisionVariable("LikesMusic",    3), // 3 possible values (True, False, Unknown) 
                new DecisionVariable("LikesSports",        3),  // 3 possible values (True, False, Unknown)
                new DecisionVariable("LikesTravelling",        3),  // 3 possible values (True, False, Unknown)
                new DecisionVariable("LikesMovies",        3)  // 3 possible values (True, False, Unknown)
            };

            tree = new DecisionTree(attributes, classCount); 
        }

        public void runAlgorithm()
        {
            prepareData();
            predicted = new int[199];

            // Create a new instance of the ID3 algorithm
            ID3Learning id3learning = new ID3Learning(tree);

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToIntArray("Sportsman", "LikesBooks", "LikesMusic", "LikesSports", "LikesTravelling", "LikesMovies");
            predicted = symbols.ToIntArray("Category").GetColumn(0);

            // Learn the training instances!
            id3learning.Run(inputs, predicted);

            // Convert to an expression tree
            expression = tree.ToExpression(); 
        }

        public int classify(FacebookProccesedEntries query)
        {
            // Compiles the expression to IL
            var func = expression.Compile();

            double[] proccessedQuery = mapElementToIntArray(query);

            try
            {
                return tree.Compute(proccessedQuery);
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public Boolean classify(FacebookProccesedEntries query, int classValue)
        {
            // Compiles the expression to IL
            var func = expression.Compile();

            double[] proccessedQuery = mapElementToIntArray(query);

            try
            {
                return func(proccessedQuery) == classValue;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void test(int classValue)
        {
            List<int> classIndex = new List<int>();
            outputs = new int[199];
            Boolean predictedValue;

            runAlgorithm();

            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            int i = 0;
            foreach (FacebookProccesedEntries entry in entries)
            {
                if (entry.Category == classValue)
                {
                    outputs[i] = 1;
                    if (!classIndex.Contains(entry.Category))
                    {
                        classIndex.Add(entry.Category);
                    }
                }
                else
                    outputs[i] = 0;

                predictedValue = classify(entry, classValue);
                if (predictedValue)
                    predicted[i] = 1;
                else
                    predicted[i] = 0;

                i++;
            }

            ConfusionMatrix matrix = new ConfusionMatrix(predicted, outputs, 1, 0);
            return;
        }

        private double[] mapElementToIntArray(FacebookProccesedEntries query)
        {
            double[] array = new double[] { Utilities.mapSportsmanWordToStatusEnum(query.Sportsman), Utilities.mapWordToStatusEnum(query.LikesBooks), Utilities.mapWordToStatusEnum(query.LikesMusic),
                                        Utilities.mapWordToStatusEnum(query.LikesSports), Utilities.mapWordToStatusEnum(query.LikesTravelling), Utilities.mapWordToStatusEnum(query.LikesMovies)};
            return array;
        }
    }
}