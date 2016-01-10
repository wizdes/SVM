using SVM_SMO.Algorithm;
using SVM_SMO.Input;

namespace SVM_SMO
{
    class Program
    {
        static void Main(string[] args)
        {
            // read the arguments and stick them into the config manager
            ConfigManager.Instance.ParseArguments(args);

            // next need to get the file name and read the elements into dense training data
            ITrainingData trainingData = new DenseTrainingData();

            // initialize the dot product kernel, stick it in the SVM algorithm 
            // TODO: use the arguments to determine the kernel to use
            // 1) dot product
            // 2) radial basis function kernel
            // 3) polynomial kernel (how to specify the degree of polynomial)
            IKernel kernel = new DotProductKernel();
            ISVMAlgorithm svm = new SMOSVMAlgorithm(trainingData, kernel);

            // SVM algorithm run
            ResultData data = svm.run();

            // print the weight vectors and 'b' constants
            data.Print();

            // record the results of the SVM run
            data.RecordResults();
        }
    }
}
