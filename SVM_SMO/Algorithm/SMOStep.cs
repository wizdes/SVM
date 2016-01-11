using SVM_SMO.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVM_SMO.Algorithm
{
    internal class SMOStep
    {
        public double E_1;
        public double E_2;

        public double y_1;
        public double y_2;

        public double s;

        public double alpha_1;
        public double alpha_2;

        public double gamma = 0;
        public double L = 0;
        public double H = 0;
        public double C = 0;
        public double epsilon = 0;

        double k11 = 0;
        double k12 = 0;
        double k22 = 0;
        double eta = 0;

        SMOSVMAlgorithm alg;

        static SMOStep smoStep;

        private SMOStep(SMOSVMAlgorithm alg)
        {
            this.alg = alg;
            C = ConfigManager.Instance.C;
            epsilon = ConfigManager.Instance.epsilon;
        }

        static public SMOStep Instance(SMOSVMAlgorithm alg)
        {
            if(smoStep == null)
            {
                smoStep = new SMOStep(alg);
            }

            return smoStep;
        }

        public bool run(int index1, int index2)
        {
            clearVariables();
            E_1 = this.alg.calculateError(index1);
            E_2 = this.alg.calculateError(index2);

            y_1 = this.alg.trainingData.GetTrainingResult(index1);
            y_2 = this.alg.trainingData.GetTrainingResult(index2);

            s = y_1 * y_2;

            alpha_1 = this.alg.calculationStore.Alphas[index1];
            alpha_2 = this.alg.calculationStore.Alphas[index2];

            gamma = 0;
            L = 0;
            H = 0;

            GetGammaAndLowHighBounds(s);

            if (L.Equals(H)) return false;

            double new_alpha_2 = CalculateNewAlpha_2(k11, k12, k22, eta);

            double alpha_difference = fabs(new_alpha_2 - alpha_2);

            if (fabs(new_alpha_2 - alpha_2) < epsilon * (new_alpha_2 + alpha_2 + epsilon)) return false;

            // here, a1 is calculated.
            double new_alpha_1 = alpha_1 - s * (new_alpha_2 - alpha_2);
            HandleAlpha1OutsideZeroC(ref new_alpha_2, ref new_alpha_1);

            // do I need to calculate W/B here?

            this.alg.calculationStore.Alphas[index1] = new_alpha_1;
            this.alg.calculationStore.Alphas[index2] = new_alpha_2;

            return true;
        }

        private void HandleAlpha1OutsideZeroC(ref double new_alpha_2, ref double new_alpha_1)
        {
            if (new_alpha_1 < 0)
            {
                new_alpha_2 += s * new_alpha_1;
                new_alpha_1 = 0;
            }
            else if (new_alpha_1 > C)
            {
                new_alpha_2 += s * (new_alpha_1 - C);
                new_alpha_1 = C;
            }
        }

        private int fabs(double v)
        {
            throw new NotImplementedException();
        }

        private double CalculateNewAlpha_2(double k11, double k12, double k22, double eta)
        {
            throw new NotImplementedException();
        }

        private void GetGammaAndLowHighBounds(double s)
        {
            if (s == 1)
            {
                gamma = alpha_1 + alpha_2;
                if (gamma > C)
                {
                    L = gamma - C;
                    H = C - gamma;
                }
                else
                {
                    L = 0;
                    H = gamma;
                }
            }
            else
            {
                gamma = alpha_1 - alpha_2;
                if (gamma > 0)
                {
                    L = 0;
                    H = C - gamma;
                }
                else
                {
                    L = -gamma;
                    H = C;
                }
            }
        }

        private void clearVariables()
        {
            throw new NotImplementedException();
        }
    }
}
