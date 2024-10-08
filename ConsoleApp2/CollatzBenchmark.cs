﻿using BenchmarkDotNet.Attributes;

public class CollatzBenchmark
{
    private static readonly Dictionary<double, int> items = new();

    private static double CalculateEven(double evenNumber) => evenNumber / 2;
    private static double CalculateOdd(double oddNumber) => (3 * oddNumber) + 1;

    private static int GetNumberOfIterationsToReachOne(double number)
    {
        int numberOfIterations = 0;
        double originalNumber = number;

        while (number != 1)
        {
            if (items.TryGetValue(number, out var cachedIterations))
            {
                numberOfIterations += cachedIterations;
                break;
            }

            if (number % 2 == 0)
                number = CalculateEven(number);
            else
                number = CalculateOdd(number);
            numberOfIterations++;
        }

        items[originalNumber] = numberOfIterations;
        return numberOfIterations;
    }

    [Benchmark]
    public void ComputeCollatzSequence()
    {
        for (var i = 1; i <= 1_000_000; i++)
            GetNumberOfIterationsToReachOne(i);

        var maxPair = items.MaxBy(item => item.Value);
        Console.WriteLine($"{maxPair.Key} - {maxPair.Value}");
    }
}
