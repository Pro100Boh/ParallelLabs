using BenchmarkDotNet.Running;
using System;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SequentialVsParallel>();

            //PrintResults();

            Console.ReadKey();
        }

        static void PrintResults()
        {
            int n = 1_000;
            var vector = new int[n];
            var random = new Random();

            for (int i = 0; i < n; i++)
            {
                vector[i] = random.Next(-10, 11);
            }

            Console.WriteLine($"Sequential = {SequentialVsParallel.CalculateNorm(vector)}");
            Console.WriteLine($"Parallel (2 threads) = {SequentialVsParallel.CalculateNormParallel(vector, 2)}");
            Console.WriteLine($"Parallel (4 threads) = {SequentialVsParallel.CalculateNormParallel(vector, 4)}");
            Console.WriteLine($"Parallel (8 threads) = {SequentialVsParallel.CalculateNormParallel(vector, 8)}");
        }



    }
}
