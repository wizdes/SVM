namespace SVM_SMO.Algorithm
{
    using Input;
    using System;
    using System.Collections.Generic;
    public class SMOSVMAlgorithm : ISVMAlgorithm
    {
        internal IKernel kernel;
        internal ITrainingData trainingData;
        internal CalculationStore calculationStore;

        public delegate int MethodDelegate(int i);

        public SMOSVMAlgorithm(ITrainingData trainingData, IKernel kernel)
        {
            this.trainingData = trainingData;
            this.kernel = kernel;
            this.calculationStore = new CalculationStore(this.trainingData);
        }

        public CalculationStore run()
        {
            // the SMO algorithm a loop; the exit conditions are:
            // if all the training elements are examined and don't affect the weights

            bool hasChanged = false;
            bool scanAllData = true;

            while (hasChanged || scanAllData)
            {
                bool changed = false;
                for (int i = 0; i < trainingData.Length; i++)
                {
                    changed = examineAndStep(i, hasChanged) ? true : hasChanged;
                }

                hasChanged = changed;

                if (scanAllData) scanAllData = false;
                else if (!hasChanged) scanAllData = true;
            }

            return this.calculationStore;
        }

        internal double calculateError(int index1)
        {
            throw new NotImplementedException();
        }

        private bool examineAndStep(int firstIndex, bool prevAlphaCalculationRequired)
        {
            if (prevAlphaCalculationRequired)
            {
                if (ContainsPreviousAlphaChanges(firstIndex))
                {
                    return false;
                }
            }

            if (RValueViolatesKKT(firstIndex))
            {
                List<Func<int, int, bool>> secondIndexSearchForAlpha = new List<Func<int, int, bool>>
                {
                    GetIndexOfHighestError,
                    GetIndexWithAlpha,
                    GetIndex
                };

                foreach (Func<int, int, bool> f in secondIndexSearchForAlpha)
                {
                    for (int i = 0; i < 0; i++)
                    {
                        if (f(firstIndex, i))
                        {
                            if (SMOStep.Instance(this).run(firstIndex, i)) return true;
                        }
                    }
                }
            }

            return false;
        }

        private void GetGammaLH(double s, ref double gamma, ref double l, ref double h)
        {
            throw new NotImplementedException();
        }

        // Helper functions to stay here

        // Helper functions that can be moved to another class (just add calcStore)
        private bool ContainsPreviousAlphaChanges(int i)
        {
            return !(this.calculationStore.Alphas[i] == 0
                || this.calculationStore.Alphas[i] == ConfigManager.Instance.C);
        }

        private bool GetIndexOfHighestError(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private bool GetIndexWithAlpha(int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private bool GetIndex(int arg1, int arg2)
        {
            return true;
        }

        private bool RValueViolatesKKT(int firstIndex)
        {
            return (RValue(firstIndex) > ConfigManager.Instance.tolerance && this.calculationStore.Alphas[firstIndex] > 0)
                            || (RValue(firstIndex) < -ConfigManager.Instance.tolerance && this.calculationStore.Alphas[firstIndex] < ConfigManager.Instance.C);
        }

        private double RValue(int i)
        {
            throw new NotImplementedException();
        }
    }
}
