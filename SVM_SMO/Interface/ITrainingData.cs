namespace SVM_SMO
{
    public interface ITrainingData
    {
        double GetTrainingData(int line, int position);

        double GetTrainingResult(int line);
    }
}
