using System;

namespace SVM_SMO.Algorithm
{
    public class CalculationStore
    {
        private ITrainingData trainingData;

        public double[] Alphas
        {
            get;
            set;
        }

        public double[] Weights
        {
            get;
            set;
        }

        public double[] B
        {
            get;
            set;
        }

        public CalculationStore(ITrainingData trainingData)
        {
            this.trainingData = trainingData;
            this.Alphas = new double[trainingData.Length];
            this.Weights = new double[trainingData.Dimension];
            this.B = new double[1];
        }

        public void CalculateWB()
        {

        }

        internal void Print()
        {
            for(int i = 0; i < this.Weights.Length; i++)
            {
                Console.WriteLine("w_" + i + ": " + this.Weights[i]);
            }

            Console.WriteLine("b: " + this.B[0]);
        }

        internal static void RecordResults()
        {
        }
    }
}
