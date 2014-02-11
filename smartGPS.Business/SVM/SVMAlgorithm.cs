using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accord;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using smartGPS.Persistance;

namespace smartGPS.Business.SVM
{
    public class SVMAlgorithm
    {
        private String userId { get; set; }
        private KernelSupportVectorMachine svm { get; set; }
        private double[][] inputs;
        private int[] outputs;
        private int[] testOutputs;

        public SVMAlgorithm(String userId)
        {
            this.userId = userId;
        }

        private void prepareData()
        {
            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            FacebookProccesedEntries entry;

            inputs = new double[199][];
            outputs = new int[199];
            testOutputs = new int[199];
            for (int i = 0; i < entries.Count(); i++)
            {
                entry = entries.ElementAt(i);
                inputs[i] = new double[6];
                inputs[i][0] = Utilities.mapWordToStatusEnum(entry.LikesBooks);
                inputs[i][1] = Utilities.mapWordToStatusEnum(entry.LikesMovies);
                inputs[i][2] = Utilities.mapWordToStatusEnum(entry.LikesMusic);
                inputs[i][3] = Utilities.mapWordToStatusEnum(entry.LikesSports);
                inputs[i][4] = Utilities.mapWordToStatusEnum(entry.LikesTravelling);
                inputs[i][5] = Utilities.mapWordToStatusEnum(entry.Sportsman);

                if (entry.Category == 0)
                    outputs[i] = -1;
                else
                    outputs[i] = 1;
            }
        }

        public void runAlgorithm()
        {
            prepareData();
            // Create a new machine with a polynomial kernel and six inputs 
            KernelSupportVectorMachine ksvm = new KernelSupportVectorMachine(new Gaussian(), 6);

            // Create the learning algorithm with the given inputs and outputs
            var smo = new SequentialMinimalOptimization(machine: ksvm, inputs: inputs, outputs: outputs)
            {
                Complexity = 100 // Create a hard-margin SVM 
            };

            bool converged = true;

            try
            {
                // Run
                double error = smo.Run();
            }
            catch (ConvergenceException)
            {
                converged = false;
            }

            int trueCategorized = 0;
            for(int i = 0; i< inputs.Length; i++)
            {
                testOutputs[i] = System.Math.Sign(ksvm.Compute(inputs[i]));

                if (testOutputs[i] == outputs[i])
                    trueCategorized++;
            }

            return;
        }

     
    }
}