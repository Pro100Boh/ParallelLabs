using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab1
{
    [SimpleJob(launchCount: 1, warmupCount: 1, targetCount: 1)]
    [HtmlExporter]
    public class SequentialVsParallel
    {
        private int[] _vector;

        [Params(1_000, 10_000, 1_000_000)]
        public int N { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _vector = new int[N];
            var random = new Random();
            
            for (int i = 0; i < N; i++)
            {
                _vector[i] = random.Next(-10, 11);
            }
        }

        [Benchmark(Description = "Sequential", Baseline = true)]
        public void CalculateNormSequential()
        {
            CalculateNorm(_vector);
        }
        
        [Benchmark(Description = "Parallel (2 threads)")]
        public void CalculateNormParallel2Threads()
        {
            CalculateNormParallel(_vector, 2);
        }

        [Benchmark(Description = "Parallel (4 threads)")]
        public void CalculateNormParallel4Threads()
        {
            CalculateNormParallel(_vector, 4);
        }

        [Benchmark(Description = "Parallel (8 threads)")]
        public void CalculateNormParallel8Threads()
        {
            CalculateNormParallel(_vector, 8);
        }

        public static int CalculateNorm(int[] vector)
        {
            return vector.Sum(Math.Abs);
        }

        public static int CalculateNormParallel(int[] vector, int threads)
        {
            int result = 0;
            int partSize = vector.Length / threads;

            var tasks = new Task[threads];

            for (int i = 0; i < threads; i++)
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
    }
}
