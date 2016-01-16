namespace SVM_SMO.Input
{
    using System;

    public class ConfigManager
    {
        static ConfigManager configManagerInstance;

        private ConfigManager()
        {
            C = 100;
            tolerance = 0.0001;
            epsilon = 0.001;
        }

        public static ConfigManager Instance
        {
            get
            {
                if (configManagerInstance == null)
                {
                    configManagerInstance = new ConfigManager();
                }

                return configManagerInstance;
            }
        }

        public double C;

        public double tolerance;

        public double epsilon;

        internal string trainingDataFilename;

        public void ParseArguments(string[] args)
        {
            if(args.Length < 2 || args.Length % 2 != 0)
            {
                throw new ArgumentException("Arguments are invalid.");
            }

            // parse string args
            for(int i = 0; i < args.Length; i = i + 2)
            {
                string option = args[i];
                string arg = args[i + 1];
                if (option[0] != '-') throw new ArgumentException("Arguments are invalid.");
                switch (option)
                {
                    case "-f":
                        trainingDataFilename = arg;
                        break;
                    default:
                        throw new ArgumentException("Option is invalid");
                }
            }
        }
    }
}
