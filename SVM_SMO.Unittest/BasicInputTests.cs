using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SVM_SMO.Unittest
{
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
    }
}
