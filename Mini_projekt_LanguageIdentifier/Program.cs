using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Mini_projekt_LanguageIdentifier
{
    internal static class Program
    {
        const int NumberOfLetters = 26;
        const double LearningRate = 0.1;
        const int Epochs = 1000;

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: System.String; size: 688MB")]
        public static void Main(string[] args)
        {
            // - - - -- - - - - - -- - - - - - - -- PROVIDE DATA - - - -- - - - - - -- - - - - - - -- 

            var dataProvider = ProvideData(out var perceptrons);

            // - - - -- - - - - - -- - - - - - - -- TRAIN - - - -- - - - - - -- - - - - - - -- 
            //train
            foreach (var kvp in dataProvider.ListOfKeyValuePairs)
            {
                List<double> vector = MakeVector(kvp.Value, NumberOfLetters); // ile liter dla tekstu

                foreach (var perceptron in perceptrons)
                {
                    for (int i = 0; i < Epochs; i++)
                    {
                        perceptron.Learn(vector, kvp.Key,
                            LearningRate); //daje wektor z tekstu, jaki to jest tak naprawde jezyk
                    }
                }
            }

            //normalize
            foreach (var perceptron in perceptrons)
            {
                perceptron.Normalize();
            }

            // - - - -- - - - - - -- - - - - - - -- TEST - - - -- - - - - - -- - - - - - - -- 
            var dataProvider2 =
                new DataProvider(
                    @"C:\Users\Jarek\RiderProjects\Mini_projekt_LanguageIdentifier\Mini_projekt_LanguageIdentifier\Data\lang.test.csv");
            var perceptronLanguage = "";
            var correct = 0;

            foreach (var kvp in dataProvider2.ListOfKeyValuePairs)
            {
                List<double> vector = MakeVector(kvp.Value, NumberOfLetters);
                NormalizeVector(vector);
                double score = 0;
                foreach (var perceptron in perceptrons)
                {
                    if (perceptron.GetNet(vector) > score)
                    {
                        score = perceptron.GetNet(vector);
                        perceptronLanguage = perceptron.language;
                    }
                }

                if (perceptronLanguage == kvp.Key) //GoodPerception
                {
                    correct++;
                }
            }

            // - - - -- - - - - - -- - - - - - - -- RESULT - - - -- - - - - - -- - - - - - - -- 

            double accuracy = (double)correct / 2;
            Console.WriteLine("Accuracy: " + accuracy + "%");
        }

        private static DataProvider ProvideData(out List<Perceptron> perceptrons)
        {
            DataProvider dataProvider =
                new DataProvider(
                    @"C:\Users\Jarek\RiderProjects\Mini_projekt_LanguageIdentifier\Mini_projekt_LanguageIdentifier\Data\lang.train.csv");

            perceptrons = new List<Perceptron>();
            foreach (var language in dataProvider.Languages)
            {
                perceptrons.Add(new Perceptron(language, NumberOfLetters));
            }

            return dataProvider;
        }


        private static List<double> MakeVector(string text, int inputSize)
        {
            var vector = new List<double>();
            for (int i = 0; i < inputSize; i++) //26 elementów/ liter
            {
                vector.Add(0);
            }

            //vector +=1 if it is the latin letter 
            foreach (var letter in text)
            {
                char lowerLetter = char.ToLower(letter);
                if (lowerLetter >= 'a' && lowerLetter <= 'z')
                {
                    int index = lowerLetter - 'a';
                    vector[index]++;
                }
            }


            return vector;
        }

        private static void NormalizeVector(List<double> vector)
        {
            // v̂ = v / |v| iteruj po vectorze, gdzie elementem jest "d" i dodaje d^2 do sumy, tworzac tak sume
            double sum = vector.Sum(d => d * d);

            var vLength = Math.Sqrt(sum); //|v|
            for (var i = 0; i < vector.Count; i++)
            {
                vector[i] /= vLength;
            }
        }
    }
}