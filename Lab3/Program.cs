using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            var arr = CreateArrayWithRandomValues(1_000_000);

            Console.WriteLine(CountOfItemsByCond(arr, x => x > 0));
            Console.WriteLine("-----------");
            Console.WriteLine(arr.Count(x => x > 0));


            //var (min, max) = GetMinMaxIndices(arr);
            //Console.WriteLine(arr[min]);
            //Console.WriteLine(arr[max]);
            //Console.WriteLine("-----------");
            //Console.WriteLine(arr.Min());
            //Console.WriteLine(arr.Max());


            //Console.WriteLine(GetChecksum(arr));
            //Console.WriteLine("-----------");
            //Console.WriteLine(arr.Aggregate((x, y) => x ^ y));

            Console.ReadKey();
        }

        static int CountOfItemsByCond(int[] arr, Func<int, bool> cond)
        {
            int counter = 0;

            arr.AsParallel().ForAll(x =>
            {
                if (cond(x))
                    Interlocked.Increment(ref counter);
            });

            return counter;
        }

        static (int min, int max) GetMinMaxIndices(int[] arr)
        {
            int indexMin = 0, indexMax = 0;

            Parallel.For(1, arr.Length, i =>
            {
                if (arr[i] < arr[indexMin])
                {
                    Interlocked.Exchange(ref indexMin, i);
                }
                else if (arr[i] > arr[indexMax])
                {
                    Interlocked.Exchange(ref indexMax, i);
                }

            });

            return (indexMin, indexMax);
        }

        static int GetChecksum(int[] arr)
        {
            int checksum = 0;

            arr.AsParallel().ForAll(x => 
            {
                int oldValue;
                int newValue;
                do
                {
                    oldValue = checksum;
                    newValue = oldValue ^ x;
                } while (Interlocked.CompareExchange(ref checksum, newValue, oldValue) != oldValue);
            });

            return checksum;
        }

        static int[] CreateArrayWithRandomValues(int size)
        {
            var random = new Random();
            var arr = new int[size];

            for (int i = 0; i < size; i++)
            {
                arr[i] = random.Next(-1_000_000_000, 1_000_000_000);
            }

            return arr;
        }
    }
}
