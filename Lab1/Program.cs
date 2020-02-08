using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static readonly Random random = new Random();

        const int minRandomValue = -10;
        const int maxRandomValue = 10;
        const int vectorLength = 10_000_000;

        static void Main(string[] args)
        {
            var vector = CreateArrayWithRandomValues(vectorLength);

            var timeSequential = SequentialAlgorithmTest(vector);

            var timeParallel = ParallelAlgorithmTest(vector, 4);

            double diff = timeSequential - timeParallel;
            Console.WriteLine($"\nDiff(ms) = {diff} ({diff / timeSequential * 100} %)");


            Console.ReadKey();
        }

        static long SequentialAlgorithmTest(int[] vector)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int result = CalculateNorm(vector);

            stopwatch.Stop();

            PrintStats("Sequential", stopwatch.ElapsedMilliseconds, result);

            return stopwatch.ElapsedMilliseconds;
        }

        static long ParallelAlgorithmTest(int[] vector, int numOfThreads)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int result = CalculateNormParallel(vector, numOfThreads);

            stopwatch.Stop();

            PrintStats($"Parallel (num of threads = {numOfThreads})", stopwatch.ElapsedMilliseconds, result);

            return stopwatch.ElapsedMilliseconds;
        }

        static int[] CreateArrayWithRandomValues(int size)
        {
            var arr = new int[size];

            for (int i = 0; i < size; i++)
            {
                arr[i] = random.Next(minRandomValue, maxRandomValue);
            }

            return arr;
        }

        static int CalculateNorm(int[] vector)
        {
            return vector.Sum(Math.Abs);
        }

        static int CalculateNormParallel(int[] vector, int numOfThreads)
        {
            int result = 0;
            int partSize = vector.Length / numOfThreads;

            Task[] tasks = new Task[numOfThreads];

            for (int i = 0; i < numOfThreads; i++)
            {
                int ii = i;

                tasks[ii] = Task.Run(() =>
                {
                    int localResult = 0;
                    for (int j = ii * partSize; j < (ii + 1) * partSize; j++)
                    {
                        localResult += Math.Abs(vector[j]);
                    }
                    Interlocked.Add(ref result, localResult);
                });
            }

            Task.WaitAll(tasks);

            return result;
        }

        static void PrintStats(string algorithmName, long time, int result)
        {
            Console.WriteLine();
            Console.WriteLine(algorithmName);
            Console.WriteLine($"Time: {time}");
            Console.WriteLine($"Result = {result}");
            Console.WriteLine();
        }
    }
}
