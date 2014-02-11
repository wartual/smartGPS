using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathNet.Numerics.Statistics;
using smartGPS.Business.ExternalServices;
using smartGPS.Persistance;

namespace smartGPS.Business.KNN
{
    public class KNNAlgorithm
    {
        private int k { get; set; }
        private String userId { get; set; }
        private List<Points> dataSet { get; set; }
        private Dictionary<int, double> coefficients { get; set; }
        private List<KeyValuePair<int, double>> sortedCoefficients { get; set; }

        public KNNAlgorithm(String userId, int k)
        {
            this.userId = userId;
            this.k = k;
            this.dataSet = new List<Points>();
        }

         public KNNAlgorithm(String userId)
        {
            this.userId = userId;
            this.k = Config.KMEANS_DEFAULT_K;
            this.dataSet = new List<Points>();
        }

         private void prepareData()
         {
             IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
             int i = 0;
             foreach (FacebookProccesedEntries entry in entries)
             {
                 Points point = new Points(mapEntryToList(entry), i, entry.Category.ToString());
                 dataSet.Add(point);
                 i++;
             }
         }

         public String runAlgorithm(FacebookProccesedEntries entry)
         {
             prepareData();

             Points point = new Points(mapEntryToList(entry), -1, null);
             double coeff;
             coefficients = new Dictionary<int, double>();

             // calculate correlation for whole set
             foreach (Points element in dataSet)
             {
                 coeff = Correlation.Pearson(point.elements, element.elements);
                 coefficients.Add(element.index, coeff);
             }

             // sort coefficients descending
             sortedCoefficients = Utilities.returnSortedKeyValuePair(coefficients);

             Dictionary<String, int> temp = new Dictionary<string,int>();
             KeyValuePair<int, double> row;
             int value;

             // k - neighbours
             for (int i = 0; i < k; i++)
             {
                 row = sortedCoefficients.ElementAt(i);

                 // calculate freqeuency of each class
                 if (temp.ContainsKey(dataSet.ElementAt(row.Key).category.ToString()))
                 {
                     value = temp[dataSet.ElementAt(row.Key).category.ToString()];
                     temp[dataSet.ElementAt(row.Key).category.ToString()] = value + 1;
                 }
                 else
                 {
                     temp.Add(dataSet.ElementAt(row.Key).category.ToString(), 1);
                 }
             }

             // return class with heighest frequency
             String classValue = Utilities.returnSortedKeyValuePair(temp).ElementAt(0).Key;
             return classValue;
         }

         private List<double> mapEntryToList(FacebookProccesedEntries entry)
         {
             List<double> element = new List<double>();
             element.Add(Utilities.mapWordToStatusEnum(entry.LikesBooks));
             element.Add(Utilities.mapWordToStatusEnum(entry.LikesMovies));
             element.Add(Utilities.mapWordToStatusEnum(entry.LikesMusic));
             element.Add(Utilities.mapWordToStatusEnum(entry.LikesSports));
             element.Add(Utilities.mapWordToStatusEnum(entry.LikesTravelling));
             element.Add(Utilities.mapWordToStatusEnum(entry.Sportsman));

             return element;
         }
    }
}