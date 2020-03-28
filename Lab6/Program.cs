using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MPI;

class Hello
{
    const int vectorLength = 1_000_000;
    const int minValue = -10;
    const int maxValue = 10;

    static void Main(string[] args)
    {
        MPI.Environment.Run(ref args, communicator =>
        {
            int rank = communicator.Rank;
            int size = communicator.Size;

            Stopwatch stopwatch = null; 

            if (rank == 0)
            {
                stopwatch = Stopwatch.StartNew();
            }

            int vectorPartLength = rank == 0 ? (vectorLength / size + vectorLength % size) : (vectorLength / size);
            int[] vectorPart = CreateArrayOfRandomValues(vectorPartLength);

            int loacalResult = vectorPart.Sum(Math.Abs);

            if (rank == 0)
            {
                int result = Enumerable.Range(1, size - 1).Select(i => communicator.Receive<int>(i, 0)).Sum();
                result += loacalResult;

                Console.WriteLine($"Result = {result}");
                Console.WriteLine($"Time = {stopwatch.ElapsedTicks}");
            }
            else
            {
                communicator.Send(loacalResult, 0, 0);
            }
        });
    }

    static int[] CreateArrayOfRandomValues(int length)
    {
        var arr = new int[length];
        var random = new Random();

        for (int i = 0; i < length; i++)
        {
            arr[i] = random.Next(minValue, maxValue + 1);
        }

        return arr;
    }

}