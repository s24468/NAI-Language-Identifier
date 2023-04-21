using System;
using System.Collections.Generic;

namespace Mini_projekt_LanguageIdentifier
{
    public class Perceptron
    {
        private List<double> weights;
        public double threshold = 1;

        public readonly string language;

        public Perceptron(string language, int inputSize)
        {
            this.language = language;
            weights = new List<double>();
            for (int i = 0; i < inputSize; i++)
            {
                weights[i] = 1;
            }
        }

        public double GetNet(List<double> vector)
        {
            double sum = 0;
            for (int i = 0; i < vector.Count; i++)
            {
                sum += vector[i] * weights[i];
            } // X * W


            return sum - threshold; //net
        }

        public void Normalize()
        {
            double sum = 0;
            foreach (var d in weights)
            {
                sum += d * d;
            }

            double norm = Math.Sqrt(sum);

            var newVector = new List<double>();

            for (int i = 0; i < weights.Count; i++)
            {
                newVector[i] = weights[i] / norm;
            }

            threshold /= norm;
            weights = newVector;
        }

        public void Learn(List<double> input, string keyLanguage, double learningRate)
        {
            int d = language == keyLanguage ? 1 : 0;

            double sum = 0;
            for (int i = 0; i < input.Count; i++) // X * W
                sum += input[i] * this.weights[i];

            int y = (sum >= this.threshold ? 1 : 0);

            if (y != d)
            {
                var newVector = new List<double>();
                for (int i = 0; i < input.Count; i++)
                {
                    newVector[i] = this.weights[i] + learningRate * (d - y) * input[i];
                } // W' = W + (Correct-Y) * Alpha * X

                weights = newVector;
                threshold += (d - y) * learningRate * -1;
            }
        }
    }
}