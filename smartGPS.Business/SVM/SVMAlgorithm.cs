using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accord;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;
using smartGPS.Persistance;

namespace smartGPS.Business.SVM
{
    public class SVMAlgorithm
    {
        private String userId { get; set; }
        private KernelSupportVectorMachine svm { get; set; }
        private double[][] inputs { get; set; }
        private int[] outputs { get; set; }
        private int[] testOutputs { get; set; }
        private MulticlassSupportVectorMachine machine { get; set; }
        private KernelSupportVectorMachine ksvm { get; set; }
        private int[] predicted { get; set; }

        public SVMAlgorithm(String userId)
        {
            this.userId = userId;
        }

        private void prepareData(Boolean isMultiClass)
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

                if (!isMultiClass)
                {
                    if (entry.Category == 0)
                        outputs[i] = -1;
                    else
                        outputs[i] = 1;
                }
                else
                {
                    outputs[i] = entry.Category;
                }
              
            }
        }

        /// TO DO
        /// 1. Multi class support http://accord.googlecode.com/svn/docs/html/T_Accord_MachineLearning_VectorMachines_MulticlassSupportVectorMachine.htm
        public void runBinaryClassAlgorithm()
        {
            prepareData(false);
         
            // Create a new machine with a polynomial kernel and six inputs 
            ksvm = new KernelSupportVectorMachine(new Gaussian(), 6);

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

            return;
        }

        public void runMultiClassAlgorithm(int classCount)
        {
            prepareData(true);

            // Create a new Linear kernel
            IKernel kernel = new Linear();

            // Create a new Multi-class Support Vector Machine with one input, 
            //  using the linear kernel and for four disjoint classes. 
            machine = new MulticlassSupportVectorMachine(6, kernel, classCount);

            // Create the Multi-class learning algorithm for the machine 
            var teacher = new MulticlassSupportVectorLearning(machine, inputs, outputs);

            // Configure the learning algorithm to use SMO to train the 
            //  underlying SVMs in each of the binary class subproblems.
            teacher.Algorithm = (svm, classInputs, classOutputs, i, j) =>
                new SequentialMinimalOptimization(svm, classInputs, classOutputs);

            bool converged = true;

            try
            {
                // Run
                double error = teacher.Run();
            }
            catch (ConvergenceException)
            {
                converged = false;
            }
        }

        public ConfusionMatrix test(int numberOfClass, Boolean isMultiClass)
        {
            if (isMultiClass)
            {
                runMultiClassAlgorithm(numberOfClass);
            }
            else
            {
                runBinaryClassAlgorithm();
            }
            
            IEnumerable<FacebookProccesedEntries> entries = FacebookDao.ProccessedFacebookEntries_getAllByUser(userId);
            int i = 0;
            predicted = new int[199];
            foreach (FacebookProccesedEntries entry in entries)
            {
                outputs[i] = entry.Category;
                if (isMultiClass)
                    predicted[i] = System.Math.Sign(machine.Compute(inputs[i]));
                else
                    predicted[i] = System.Math.Sign(ksvm.Compute(inputs[i]));
                i++;
            }

            ConfusionMatrix matrix = new ConfusionMatrix(predicted, outputs, 1, 0);
            return matrix;
        }

        public int clasify(FacebookProccesedEntries entry, Boolean isMultiClass)
        {
            double[] proccessedInputs = new double[6];
            proccessedInputs[0] = Utilities.mapWordToStatusEnum(entry.LikesBooks);
            proccessedInputs[1] = Utilities.mapWordToStatusEnum(entry.LikesMovies);
            proccessedInputs[2] = Utilities.mapWordToStatusEnum(entry.LikesMusic);
            proccessedInputs[3] = Utilities.mapWordToStatusEnum(entry.LikesSports);
            proccessedInputs[4] = Utilities.mapWordToStatusEnum(entry.LikesTravelling);
            proccessedInputs[5] = Utilities.mapWordToStatusEnum(entry.Sportsman);

            if (isMultiClass)
                return System.Math.Sign(machine.Compute(proccessedInputs));
            else
                return System.Math.Sign(ksvm.Compute(proccessedInputs));
        }
    }
}