using System;
using System.Collections.Generic;

namespace Mini_projekt_LanguageIdentifier
{
    public class Perceptron
    {
        private List<double> weights;
        public double threshold = 1;

        public string language;

        public Perceptron(string language, int inputSize)
        {
            this.language = language;
            weights = new List<double>();
            for (int i = 0; i < inputSize; i++)
            {
                weights.Add(1);
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
                newVector.Add(weights[i] / norm);
            }

            threshold /= norm;
            weights = newVector;
        }

        public void Learn(List<double> input, string keyLanguage, double learningRate)
        {
            int d = language == keyLanguage ? 1 : 0;
            double sum = 0;
            for (int i = 0; i < input.Count; i++)
            {
                sum += input[i] * this.weights[i];
            } // X * W

            int y = (sum >= threshold ? 1 : 0);
            if (y != d)
            {
                var newVector = new List<double>();

                for (int i = 0; i < input.Count; i++)
                {
                    newVector.Add(this.weights[i] + (d - y) * learningRate * input[i]);
                } // W' = W + (Correct-Y) * Alpha * X

                weights = newVector;
                threshold += (d - y) * learningRate * -1;
            }
        }
    }
}