using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5
{
	class Program
	{
		static readonly Random _random = new Random();

		static void Main(string[] args)
		{
			var arr1Task = Task.Run(() => GetFirstArray(10_000));
			var arr2Task = Task.Run(() => GetSecondArray(10_000));

			Task.WaitAll(arr1Task, arr2Task);

			var arr1 = arr1Task.Result;
			var arr2 = arr2Task.Result;

			var arr3 = arr1.AsParallel().Intersect(arr2.AsParallel()).OrderBy(x => x).ToArray();

			PrintCollection(arr3);
		}

		static int[] GetFirstArray(int size)
		{
			var arr = CreateArrayOfRandomValues(size);

			double val = 0.6 * arr.Max();

			arr = arr.AsParallel().Where(x => x > val).OrderBy(x => x).ToArray();

			return arr;
		}

		static int[] GetSecondArray(int size)
		{
			var arr = CreateArrayOfRandomValues(size);

			arr = arr.AsParallel().Where(x => x % 2 == 0).OrderBy(x => x).ToArray();

			return arr;
		}


		static int[] CreateArrayOfRandomValues(int size)
		{
			var arr = new int[size];

			for (int i = 0; i < size; i++)
			{
				arr[i] = _random.Next(-10_000, 10_000);
			}

			return arr;
		}

		static void PrintCollection<T>(IEnumerable<T> collection)
		{
			Console.WriteLine(string.Join(", ", collection));
			Console.WriteLine();
		}
	}
}
