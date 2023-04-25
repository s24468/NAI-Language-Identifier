using System;
using System.Collections.Generic;
using System.Linq;

namespace Mini_projekt_LanguageIdentifier
{
    public class Perceptron
    {
        private List<double> _weights;
        public double Threshold = 1;//próg

        public string language;

        public Perceptron(string language, int inputSize)
        {
            this.language = language;
            _weights = new List<double>();
            for (var i = 0; i < inputSize; i++)
            {
                _weights.Add(1);
            }
        }

        public double GetNet(List<double> vector)
        {
            double sum = 0;
            for (var i = 0; i < vector.Count; i++)// X * W
            {
                sum += vector[i] * _weights[i];//ogólne net
            } 

            return sum - Threshold; //uwzględniony próg
        }

        public void Normalize()
        {
            // v̂ = v / |v| iteruj po vectorze, gdzie elementem jest "d" i dodaje d^2 do sumy, tworzac tak sume
            var sum = _weights.Sum(d => d * d);

            var vLength = Math.Sqrt(sum);//|v|

            var newVector = new List<double>();

            for (var i = 0; i < _weights.Count; i++)
            {
                newVector.Add(_weights[i] / vLength);//v[i] / |v|
            }

            Threshold /= vLength;
            _weights = newVector;
        }

        public void Learn(List<double> input, string keyLanguage, double learningRate)
        {
            var correct = language == keyLanguage ? 1 : 0;
            double sum = 0;
            for (var i = 0; i < input.Count; i++)
            {
                sum += input[i] * _weights[i];
            }

            var y = (sum >= Threshold ? 1 : 0);
            if (y != correct)
            {
                var newVector = new List<double>();

                for (int i = 0; i < input.Count; i++) // W' = W + (Correct-Y) * Alpha * X
                {
                    newVector.Add(_weights[i] + (correct - y) * learningRate * input[i]);
                }

                _weights = newVector;
                Threshold -= (correct - y) * learningRate ;
            }
        }
    }
}