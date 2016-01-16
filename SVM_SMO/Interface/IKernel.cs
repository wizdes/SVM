namespace SVM_SMO
{
    public interface IKernel
    {
        double calculate(ITrainingData data, int index1, int index2);
    }
}
