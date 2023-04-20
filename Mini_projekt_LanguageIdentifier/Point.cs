using System;
using System.Collections.Generic;
using System.Linq;

public class Point
{
    internal List<double> numbers { get; set; }
    internal string name { get; set; }

    public Point(string name, List<double> numbers)
    {
        this.name = name;
        this.numbers = numbers;
    }
}