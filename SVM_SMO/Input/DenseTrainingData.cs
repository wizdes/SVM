namespace SVM_SMO.Input
{
    using System;
    using System.IO;
    using System.Linq;
    public class DenseTrainingData : ITrainingData
    {
        private double[,] trainingValues;
        private int[] result;

        private IFileManager fileManager;
        
        public DenseTrainingData(IFileManager providedFileManager = null)
        {
            // read line by line
            string path = ConfigManager.Instance.trainingDataFilename;

            if (providedFileManager == null) fileManager = new FileManager(path);
            else fileManager = providedFileManager;

            string line;
            int numLines = 0;
            int numParam = -1;
            // Read the file and display it line by line.
            while ((line = fileManager.ReadLine()) != null && !string.IsNullOrEmpty(line))
            {
                if(numParam == -1)
                {
                    numParam = line.Split(' ').Length - 1;
                }
                else if(line.Split(' ').Length != numParam + 1)
                {
                    throw new ArgumentException("Bad training data, num parameters is different.");
                }

                numLines++;
            }

            if(numLines < 1 && numParam < 1)
            {
                throw new ArgumentException("Training data is not available.");
            }

            trainingValues = new double[numLines, numParam];
            result = new int[numLines];
            // now read it, and then have it as trainingValues

            fileManager.ResetReader();

            int lineCounter = 0;
            while ((line = fileManager.ReadLine()) != null && !string.IsNullOrEmpty(line))
            {
                double[] values = line.Split(' ').Select(x => Convert.ToDouble(x)).ToArray();
                int i = 0;
                for (i = 0; i < values.Length - 1; i++)
                {
                    trainingValues[lineCounter, i] = values[i];
                }

                result[lineCounter] = Convert.ToInt32(values[i]);
                lineCounter++;
            }
        }

        public double GetTrainingData(int line, int position)
        {
            return trainingValues[line, position];
        }

        public double GetTrainingResult(int line)
        {
            return result[line];
        }
    }
}
