namespace SVM_SMO.Input
{
    using System.IO;

    public class FileManager : IFileManager
    {
        private string path;
        StreamReader sr;

        public FileManager(string path)
        {
            this.path = path;
            sr = new StreamReader(path);
        }

        public string ReadLine()
        {
            return sr.ReadLine();
        }

        public void ResetReader()
        {
            sr.DiscardBufferedData();
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        ~FileManager()
        {
            sr.Close();
        }
    }
}
