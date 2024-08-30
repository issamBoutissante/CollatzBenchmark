using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class CollatzBenchmark
{
    private static readonly HashSet<CollatzItem> items = new();

    private static double CalculateEven(double evenNumber) => evenNumber / 2;
    private static double CalculateOdd(double oddNumber) => (3 * oddNumber) + 1;

    private static int GetNumberOfIterationsToReachOne(double number)
    {
        int numberOfIterations = 1;

        // Create a new CollatzItem to search for it in the HashSet
        var itemToSearch = new CollatzItem(number);

        // Check if this number already exists, and if so, use its stored NumberOfIterations
        if (items.TryGetValue(itemToSearch, out var existingItem))
        {
            return numberOfIterations + existingItem.NumberOfIterations;
        }

        do
        {
            if (number % 2 == 0)
                number = CalculateEven(number);
            else
                number = CalculateOdd(number);
            numberOfIterations++;
        } while (number != 1);

        return numberOfIterations;
    }

    [Benchmark]
    public void ComputeCollatzSequence()
    {
        for (var i = 1; i <= 1_000_000; i++)
        {
            var collatzItem = new CollatzItem(i);
            collatzItem.NumberOfIterations = GetNumberOfIterationsToReachOne(i);
            items.Add(collatzItem);
        }

        var maxPair = items.MaxBy(item => item.NumberOfIterations);
        Console.WriteLine($"{maxPair?.StartNumber} - {maxPair?.NumberOfIterations}");
    }
}

public class CollatzItem
{
    public double StartNumber { get; set; }
    public int NumberOfIterations { get; set; }

    public CollatzItem(double startNumber)
    {
        StartNumber = startNumber;
    }

    public override int GetHashCode() => StartNumber.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is not CollatzItem other) return false;
        return StartNumber == other.StartNumber;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<CollatzBenchmark>();
        //new CollatzBenchmark().ComputeCollatzSequence();
    }
}
