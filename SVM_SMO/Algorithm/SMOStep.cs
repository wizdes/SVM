namespace SVM_SMO.Algorithm
{
    using SVM_SMO.Input;
    using System;

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

            double new_alpha_2 = CalculateNewAlpha_2(k11, k12, k22, eta, E_1, E_2, y_1, y_2, alpha_2);

            double alpha_difference = this.alg.abs(new_alpha_2 - alpha_2);

            if (this.alg.abs(new_alpha_2 - alpha_2) < epsilon * (new_alpha_2 + alpha_2 + epsilon)) return false;

            // here, a1 is calculated.
            double new_alpha_1 = alpha_1 - s * (new_alpha_2 - alpha_2);
            HandleAlpha1OutsideZeroC(ref new_alpha_2, ref new_alpha_1);

            // do I need to calculate W/B here?
            // yes, you do, for the error calculations.

            double bnew;

            if (new_alpha_1 > 0 && new_alpha_1 < C)
                bnew = this.alg.calculationStore.B[0] + E_1 + y_1 * (new_alpha_1 - alpha_1) * k11 + y_2 * (new_alpha_2 - alpha_2) * k12;
            else {
                if (new_alpha_2 > 0 && new_alpha_2 < C)
                    bnew = this.alg.calculationStore.B[0] + E_2 + y_1 * (new_alpha_1 - alpha_1) * k12 + y_2 * (new_alpha_2 - alpha_2) * k22;
                else {
                    double b1 = this.alg.calculationStore.B[0] + E_1 + y_1 * (new_alpha_1 - alpha_1) * k11 + y_2 * (new_alpha_2 - alpha_2) * k12;
                    double b2 = this.alg.calculationStore.B[0] + E_2 + y_1 * (new_alpha_1 - alpha_1) * k12 + y_2 * (new_alpha_2 - alpha_2) * k22;
                    bnew = (b1 + b2) / 2;
                }
            }

            this.alg.calculationStore.B[0] = bnew;

            double t1 = y_1 * (new_alpha_1 - alpha_1);
            double t2 = y_2 * (new_alpha_2 - alpha_2);


            for (int i = 0; i < this.alg.calculationStore.Weights.Length; i++)
                this.alg.calculationStore.Weights[i] += this.alg.trainingData.GetTrainingData(index1, i) * t1 + this.alg.trainingData.GetTrainingData(index2, i) * t2;

            this.alg.calculationStore.Alphas[index1] = new_alpha_1;
            this.alg.calculationStore.Alphas[index2] = new_alpha_2;

            return true;
        }

        private double CalculateNewAlpha_2(double k11, double k12, double k22, double eta, double e_1, double e_2, double y_1, double y_2, double alpha_2)
        {
            double a2 = 0;
            // a1 and a2 are calculated here
            // depending on e2, e1, the eta and the previous alpha, a2 is determined to be either L or H.
            if (eta < 0)
            {
                a2 = alpha_2 + y_2 * (E_2 - E_1) / eta;
                if (a2 < L)
                    a2 = L;
                else if (a2 > H)
                    a2 = H;
            }
            else {

                // compute Lobj, Hobj; objective function at a2 = L, a2 = H
                // I'm guessing this will make sense once I know what LObj and HObj are.
                double c1 = eta / 2;
                double c2 = y_2 * (E_1 - E_2) - eta * alpha_2;
                double Lobj = c1 * L * L + c2 * L;
                double Hobj = c1 * H * H + c2 * H;

                if (Lobj > Hobj + epsilon) a2 = L;
                else if (Lobj < Hobj - epsilon) a2 = H;
                else a2 = alpha_2;
            }

            return a2;
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
            E_1 = 0;
            E_2 = 0;

            y_1 = 0;
            y_2 = 0;

            s = 0;

            alpha_1 = 0;
            alpha_2 = 0;

            gamma = 0;
            L = 0;
            H = 0;
            C = 0;
            epsilon = 0;

            k11 = 0;
            k12 = 0;
            k22 = 0;
            eta = 0;
        }
    }
}
