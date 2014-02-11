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

        public int runAlgorithm()
        {
            prepareData();
            
            // Create a new instance of the ID3 algorithm
            ID3Learning id3learning = new ID3Learning(tree);

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToIntArray("Sportsman", "LikesBooks", "LikesMusic", "LikesSports", "LikesTravelling", "LikesMovies");
            int[] outputs = symbols.ToIntArray("Category").GetColumn(0);

            // Learn the training instances!
            id3learning.Run(inputs, outputs);

            // Convert to an expression tree
            var expression = tree.ToExpression();

            // Compiles the expression to IL
            var func = expression.Compile();
            int m = tree.Compute(mapElementToIntArray());
            return 1;
        }

        public void test()
        {
            String classValue;
            int categorized = 0;
            int trueCategorized = 0;

            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);

            foreach (FacebookProccesedEntries entry in entries)
            {
                classValue = tree.Compute(mapElementToIntArray()).ToString();

                if (!classValue.Equals(""))
                {
                    categorized++;
                }

                if (classValue.Equals(entry.Category.ToString()))
                {
                    trueCategorized++;
                }
            }
        }

        private int[] mapElementToIntArray()
        {
            int[] array = new int[] { Utilities.mapWordToStatusEnum(element.Sportsman), Utilities.mapWordToStatusEnum(element.LikesBooks), Utilities.mapWordToStatusEnum(element.LikesMusic),
                                        Utilities.mapWordToStatusEnum(element.LikesSports), Utilities.mapWordToStatusEnum(element.LikesTravelling), Utilities.mapWordToStatusEnum(element.LikesMovies)};
            return array;
        }
    }
}