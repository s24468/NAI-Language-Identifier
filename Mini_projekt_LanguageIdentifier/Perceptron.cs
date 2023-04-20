using System;
using System.Collections.Generic;
using System.Linq;

public class Perceptron
{
    private List<double> weights;
    private double theta;

    public Perceptron(int lenOfVectorNumbers)
    {
        Random rand = new Random();
        weights = new List<double>();
        for (int i = 0; i < lenOfVectorNumbers; i++)
        {
            weights.Add(Math.Round(rand.NextDouble(), 7)); // weights random between [0;1]
        }

        theta = Math.Round(rand.NextDouble(), 7);
    }


    public int Predict(List<double> inputs)
    {
        double dotProduct = 0;
        for (int i = 0; i < inputs.Count; i++)
        {
            dotProduct += inputs.ElementAt(i) * weights.ElementAt(i);
        }

        return (dotProduct - theta >= 0) ? 1 : 0; //return 1 if dot product is positive, -1 if negative
    }


    public void Train(List<Point> trainingData, int numEpochs, double learningRate)
    {
        for (int epoch = 0; epoch < numEpochs; epoch++)
        {
            foreach (var point in trainingData)
            {
                var target =
                    (point.name == "Iris-virginica")
                        ? 1
                        : 0; //set target output to 1 if example is of virginica class, 0 if versicolor
                var prediction = Predict(point.numbers);
                if (prediction != target) //update weights if prediction is incorrect
                {
                    theta -= learningRate * (target - prediction);
                    for (int i = 0; i < weights.Count; i++)
                    {
                        weights[i] += learningRate * (target - prediction) * point.numbers.ElementAt(i);
                    }
                }
            }
        }

        Console.WriteLine("odchylenie: " + theta);
    }


    private void showWeights()
    {
        for (int i = 0; i < weights.Count; i++)
        {
            Console.Write(weights.ElementAt(i) + " ");
        }

        Console.WriteLine();
    }
}