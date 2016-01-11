namespace SVM_SMO
{
    public interface ITrainingData
    {
        int Length { get; }

        double GetTrainingData(int line, int position);

        double GetTrainingResult(int line);
    }
}
