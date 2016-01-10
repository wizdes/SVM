namespace SVM_SMO.Algorithm
{
    using System;

    public class SMOSVMAlgorithm : ISVMAlgorithm
    {
        private IKernel kernel;
        private ITrainingData trainingData;

        public SMOSVMAlgorithm(ITrainingData trainingData, IKernel kernel)
        {
            this.trainingData = trainingData;
            this.kernel = kernel;
        }

        public ResultData run()
        {
            throw new NotImplementedException();
        }
    }
}
