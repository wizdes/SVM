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
            this.Weights = new double[trainingData.Length];
            this.B = new double[trainingData.Length];
        }

        public void CalculateWB()
        {

        }

        internal static void Print()
        {
            throw new NotImplementedException();
        }

        internal static void RecordResults()
        {
            throw new NotImplementedException();
        }
    }
}
