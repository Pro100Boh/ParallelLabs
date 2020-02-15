using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace Lab1
{
    class Program
    {
        static readonly Random random = new Random();

        const int minRandomValue = -10;
        const int maxRandomValue = 10;

        const string path = "results.csv";
        static StreamWriter streamWriter;

        static void Main(string[] args)
        {
            File.Create(path).Close();
            streamWriter = new StreamWriter(path, true);
            streamWriter.WriteLine("Method,Vector length,Result,Time,Speedup factor,Efficiency");

            foreach (int vectorLength in new[] { 1_000, 10_000, 10_000_000 })
            {
                var vector = CreateArrayWithRandomValues(vectorLength);
                var timeSequential = SequentialAlgorithmTest(vector);

                ParallelAlgorithmTest(vector, timeSequential, 2);
                ParallelAlgorithmTest(vector, timeSequential, 4);
                ParallelAlgorithmTest(vector, timeSequential, 8);
            }

            streamWriter.Close();
        }

        static long SequentialAlgorithmTest(int[] vector)
        {
            var stopwatch = Stopwatch.StartNew();

            int result = CalculateNorm(vector);

            stopwatch.Stop();

            LogResults("Sequential", vector.Length, stopwatch.ElapsedTicks, result);

            return stopwatch.ElapsedTicks;
        }

        static void ParallelAlgorithmTest(int[] vector, long timeSequential, int threads)
        {
            var stopwatch = Stopwatch.StartNew();

            int result = CalculateNormParallel(vector, threads);

            stopwatch.Stop();

            double speedupFactor = (double)timeSequential / stopwatch.ElapsedTicks;
            double efficiency = speedupFactor / threads;

            LogResults($"Parallel {threads} threads", vector.Length, stopwatch.ElapsedTicks, result, speedupFactor, efficiency);
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

            var threads = new Thread[numOfThreads];

            for (int i = 0; i < numOfThreads; i++)
            {
                int ii = i;

                threads[ii] = new Thread(() =>
                {
                    int localResult = 0;
                    for (int j = ii * partSize; j < (ii + 1) * partSize; j++)
                    {
                        localResult += Math.Abs(vector[j]);
                    }
                    Interlocked.Add(ref result, localResult);
                });
                threads[ii].Start();
            }

            foreach (var thread in threads)
                thread.Join();

            return result;
        }

        static void LogResults(string algorithmName, int vectorLength, long time, int result, double speedupFactor = 1.0, double efficiency = 1.0)
        {
            string log = $"{algorithmName},{vectorLength},{result},{time},{speedupFactor.ToString("0.00", CultureInfo.InvariantCulture)},{efficiency.ToString("0.00", CultureInfo.InvariantCulture)}";

            streamWriter.WriteLine(log);
        }

    }
}
