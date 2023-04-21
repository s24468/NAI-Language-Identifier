using System;
using System.Collections.Generic;

namespace Mini_projekt_LanguageIdentifier
{
    internal static class Program
    {
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
            foreach (var kvp in dataProvider.Dictionary)
            {
                // Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
                List<double> vector = MakeVector(kvp.Value);

                foreach (var perceptron in perceptrons)
                {
                    for (int i = 0; i < epochs; i++)
                    {
                        perceptron.Learn(vector, kvp.Value, learningRate);
                    }
                }
            }

            //normalize
            foreach (var perceptron in perceptrons)
            {
                perceptron.Normalize();
            }

            // - - - -- - - - - - -- - - - - - - -- TEST - - - -- - - - - - -- - - - - - - -- 
            dataProvider =
                new DataProvider(
                    @"C:\Users\Jarek\RiderProjects\Mini_projekt_LanguageIdentifier\Mini_projekt_LanguageIdentifier\Data\lang.test.csv");
            string perceptronLanguage = "";
            int correct = 0;

            foreach (var kvp in dataProvider.Dictionary)
            {
                // Console.WriteLine("Key: {0}, Value: {1}", kvp.Key, kvp.Value);
                List<double> vector = MakeVector(kvp.Value);
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

            // for (String testDatum : testData) {
            //     parts = testDatum.split(",");
            //     language = parts[0];
            //     text = parts[1];
            //
            //     List<double> vector = makeVector(text, numberOfLetters);
            //
            //     normalize(vector);
            //     score = -1;
            //
            //     for (Perceptron perceptron : perceptrons) {
            //         if (perceptron.getNet(vector) > score) {
            //             score = perceptron.getNet(vector);
            //             perceptronLanguage = perceptron.getLanguage();
            //         }
            //     }
            //     if (perceptronLanguage.equals(language)) {
            //         correct++;
            //         System.out.println("+ Good Perception: " + perceptronLanguage + " - " + text);
            //         System.out.println();
            //     }else{
            //         System.out.println("- Bad Perceptron: " +  perceptronLanguage + " - " + text);
            //         System.out.println();
            //     }
            //
            // }
            // double accuracy = (double) correct / 200 * 100;
            // System.out.println("Accuracy: " + accuracy + "%");
        }


        private static List<double> MakeVector(String text)
        {
            var vector = new List<double>();

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