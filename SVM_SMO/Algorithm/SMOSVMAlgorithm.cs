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
            double calculatedResult = 0;

            for (int i = 0; i < this.calculationStore.Weights.Length; i++)
            {
                calculatedResult +=
                    this.calculationStore.Weights[i] * this.trainingData.GetTrainingData(index1, i);

            }

            calculatedResult -= this.calculationStore.B[0];
            return calculatedResult;
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
                    for (int i = 0; i < this.calculationStore.Alphas.Length; i++)
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

        // Helper functions that can be moved to another class (just add calcStore)
        private bool ContainsPreviousAlphaChanges(int i)
        {
            return !(this.calculationStore.Alphas[i] == 0
                || this.calculationStore.Alphas[i] == ConfigManager.Instance.C);
        }

        public double abs(double x)
        {
            return x > 0 ? x : -x;
        }

        private bool GetIndexOfHighestError(int arg1, int arg2)
        {
            double maxError = 0;
            double maxErrorIndex = -1;
            for(int i = 0; i < this.calculationStore.Alphas.Length; i++)
            {
                double errorDiff = abs(calculateError(i) - calculateError(arg1));
                if (errorDiff > maxError)
                {
                    maxError = errorDiff;
                    maxErrorIndex = i;
                }
            }

            if (maxErrorIndex == -1) return false;
            return true;
        }

        private bool GetIndexWithAlpha(int arg1, int arg2)
        {
            int index2 = arg1 + arg2 % this.calculationStore.Alphas.Length;
            double ind2Alpha = this.calculationStore.Alphas[index2];
            if(ind2Alpha > 0 && ind2Alpha < ConfigManager.Instance.C)
            {
                return true;
            }

            return false;
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
            double e = calculateError(i);
            return this.trainingData.GetTrainingResult(i) * (e - this.trainingData.GetTrainingResult(i));
        }
    }
}
