using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

internal class Program
{
    public static void Main(string[] args)
    {
    }
    
    private static void LoadData(string path, List<Point> pointList)
    {
        NumberFormatInfo provider = new NumberFormatInfo();
        provider.NumberDecimalSeparator = ".";
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string[] columns = line.Split(',');
            List<double> numbers = new List<double>();
            for (int i = 0; i < columns.Length - 1; i++)
            {
                numbers.Add(Convert.ToDouble(columns[i], provider));
            }

            pointList.Add(new Point(columns[columns.Length - 1], numbers));
        }
    }
}