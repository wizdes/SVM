using System;

namespace SVM_SMO.Algorithm
{
    public class DotProductKernel : IKernel
    {
        public double calculate(ITrainingData data, int index1, int index2)
        {
            double dotProduct = 0;
            for(int i = 0; i < data.Dimension; i++)
            {
                dotProduct += data.GetTrainingData(index1, i) * data.GetTrainingData(index2, i);
            }
            return dotProduct;
        }
    }
}
