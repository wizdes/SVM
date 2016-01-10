using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SVM_SMO.Unittest
{
    using System;
    using SVM_SMO.Input;

    [TestClass]
    public class BasicInputTests
    {
        [TestMethod]
        public void ConfigManagerBasicInputTest()
        {
            string testStrInput = "-f filename.txt";
            ConfigManager.Instance.ParseArguments(testStrInput.Split(' '));
            Assert.AreEqual("filename.txt", ConfigManager.Instance.trainingDataFilename);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrainingDataArgumentExceptionTest()
        {
            int lineIndex = 0;
            IFileManager testFileManager = new SVM_SMO.Fakes.StubIFileManager()
            {
                ReadLine = () =>
                {
                    if (lineIndex++ == 0) return "";
                    else return null;
                },
                ResetReader = () => { lineIndex = 0; }
            };

            ITrainingData trainingData = new DenseTrainingData(testFileManager);
        }

        [TestMethod]
        public void TrainingDataTest()
        {
            int lineIndex = 0;
            string[] lines = new [] { "0 1 2 1", "2 -4 1.5 -1", "" };
            IFileManager testFileManager = new SVM_SMO.Fakes.StubIFileManager()
            {
                ReadLine = () => { return lines[lineIndex++]; },
                ResetReader = () => { lineIndex = 0; }
            };

            ITrainingData trainingData = new DenseTrainingData(testFileManager);

            Assert.AreEqual(0, trainingData.GetTrainingData(0, 0));
            Assert.AreEqual(1, trainingData.GetTrainingData(0, 1));
            Assert.AreEqual(2, trainingData.GetTrainingData(0, 2));
            Assert.AreEqual(1, trainingData.GetTrainingResult(0));

            Assert.AreEqual(2,   trainingData.GetTrainingData(1, 0));
            Assert.AreEqual(-4,  trainingData.GetTrainingData(1, 1));
            Assert.AreEqual(1.5, trainingData.GetTrainingData(1, 2));
            Assert.AreEqual(-1,  trainingData.GetTrainingResult(1));
        }
    }
}
