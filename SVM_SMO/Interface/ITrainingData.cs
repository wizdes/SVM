namespace SVM_SMO
{
    public interface ITrainingData
    {
        int Length { get; }

        int Dimension { get; }

        double GetTrainingData(int line, int position);

        double GetTrainingResult(int line);
    }
}
