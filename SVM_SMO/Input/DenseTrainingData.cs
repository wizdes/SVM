namespace SVM_SMO.Input
{
    using System;
    using System.IO;
    public class DenseTrainingData : ITrainingData
    {
        double[,] trainingValues;
        int[] result;
        
        public DenseTrainingData()
        {
            // read line by line
            string path = ConfigManager.Instance.trainingDataFilename;

            string line;
            int counter = 0;
            int numParam = -1;
            // Read the file and display it line by line.
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                if(numParam == -1)
                {
                    numParam = line.Split(' ').Length;
                }
                else if(line.Split(' ').Length != numParam)
                {
                    throw new ArgumentException("Bad training data, num parameters is different.");
                }

                Console.WriteLine(line);
                counter++;
            }

            file.Close();

            trainingValues = new double[counter, numParam];
            result = new int[counter];
            // now read it, and then have it as trainingValues
        }
    }
}
