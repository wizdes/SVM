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

        public CalculationStore(ITrainingData trainingData)
        {
            this.trainingData = trainingData;
            this.Alphas = new double[trainingData.Length];
        }
    }
}
