namespace SVM_SMO.Input
{
    using System;
    using System.IO;
    using System.Linq;
    public class DenseTrainingData : ITrainingData
    {
        double[,] trainingValues;
        int[] result;

        private IFileManager fileManager;
        
        public DenseTrainingData(IFileManager providedFileManager = null)
        {
            // read line by line
            string path = ConfigManager.Instance.trainingDataFilename;

            if (providedFileManager == null) fileManager = new FileManager(path);

            string line;
            int numLines = 0;
            int numParam = -1;
            // Read the file and display it line by line.
            while ((line = fileManager.ReadLine()) != null)
            {
                if(numParam == -1)
                {
                    numParam = line.Split(' ').Length;
                }
                else if(line.Split(' ').Length != numParam)
                {
                    throw new ArgumentException("Bad training data, num parameters is different.");
                }

                numLines++;
            }

            trainingValues = new double[numLines, numParam];
            result = new int[numLines];
            // now read it, and then have it as trainingValues

            fileManager.ResetReader();

            int lineCounter = 0;
            while ((line = fileManager.ReadLine()) != null)
            {
                double[] values = line.Split(' ').Select(x => Convert.ToDouble(x)).ToArray();
                for(int i = 0; i < values.Length; i++)
                {
                    trainingValues[lineCounter, i] = values[i];
                }

                lineCounter++;
            }
        }
    }
}
