using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Mini_projekt_LanguageIdentifier
{
    internal static class Program
    {
        // [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        //     MessageId = "type: System.String; size: 571MB")]
        // [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        //     MessageId = "type: System.String; size: 693MB")]
        public static void Main(string[] args)
        {
            // - - - -- - - - - - -- - - - - - - -- PROVIDE DATA - - - -- - - - - - -- - - - - - - -- 

            int numberOfLetters = 26;
            double learningRate = 0.01;
            int epochs = 1000;
            DataProvider dataProvider =
                new DataProvider(
                    @"C:\Users\Jarek\RiderProjects\Mini_projekt_LanguageIdentifier\Mini_projekt_LanguageIdentifier\Data\lang.train.csv");

            var perceptrons = new List<Perceptron>();
            foreach (var language in dataProvider.Languages)
            {
                perceptrons.Add(new Perceptron(language, numberOfLetters));

            }

            // - - - -- - - - - - -- - - - - - - -- TRAIN - - - -- - - - - - -- - - - - - - -- 
            //train
            foreach (var kvp in dataProvider.ListOfKeyValuePairs)
            {
                List<double> vector = MakeVector(kvp.Value, numberOfLetters);

                foreach (var perceptron in perceptrons)
                {
                    for (int i = 0; i < epochs; i++)
                    {
                        perceptron.Learn(vector, kvp.Key, learningRate);
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
            string perceptronLanguage = "";
            int correct = 0;

            foreach (var kvp in dataProvider2.ListOfKeyValuePairs)
            {
                // Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
                List<double> vector = MakeVector(kvp.Value, numberOfLetters);
                NormalizeVector(vector);
                double score = -1;
                foreach (var perceptron in perceptrons)
                {
                    if (perceptron.GetNet(vector) > score)
                    {
                        score = perceptron.GetNet(vector);
                        perceptronLanguage = perceptron.language;
                    }
                }

                if (perceptronLanguage == kvp.Key)
                {
                    correct++;
                    Console.WriteLine("+ Good Perception: " + perceptronLanguage + " - " + kvp.Value);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("- Bad Perceptron: " + perceptronLanguage + " - " + kvp.Value);
                    Console.WriteLine();
                }
            }

            // - - - -- - - - - - -- - - - - - - -- RESULT - - - -- - - - - - -- - - - - - - -- 

            double accuracy = (double)correct / 200 * 100;
            Console.WriteLine("Accuracy: " + accuracy + "%");
        }


        private static List<double> MakeVector(string text, int inputSize)
        {
            var vector = new List<double>();
            for (int i = 0; i < inputSize; i++)
            {
                vector.Add(0);
            }

            foreach (var letter in text.ToCharArray())
            {
                if (letter >= 'a' && letter <= 'z')
                {
                    int index = letter - 'a';
                    vector[index]++;
                }
                else if (letter >= 'A' && letter <= 'Z')
                {
                    int index = letter - 'A';
                    vector[index]++;
                }
            }

            return vector;
        }

        private static void NormalizeVector(List<double> vector)
        {
            //     // v̂ = v / |v|
            double sum = 0;

            foreach (var d in vector)
            {
                sum += d * d;
            }

            double norm = Math.Sqrt(sum);
            for (int i = 0; i < vector.Count; i++)
            {
                vector[i] /= norm;
            }
        }
    }
}