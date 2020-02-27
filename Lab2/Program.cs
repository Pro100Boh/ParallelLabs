using System;
using System.Threading;

namespace Lab2
{

    class Program
    {
        static readonly Random _random = new Random();

        static void Main(string[] args)
        {
            const int numOfProcesses = 100;
            const int cpuMinTimeOfProcess = 10;
            const int cpuMaxTimeOfProcess = 50;
            const int processGenerationIntervalMin = 2;
            const int processGenerationIntervalMax = 20;

            var cpuQueue = new CPUQueue();
            var cpu = new CPU(cpuMinTimeOfProcess, cpuMaxTimeOfProcess, cpuQueue);

            var thread = new Thread(cpu.StartProcessing);
            thread.Start();

            for (int i = 0; i < numOfProcesses; i++)
            {
                cpu.AddProcessToQueue(new CPUProcess(i));
                Thread.Sleep(_random.Next(processGenerationIntervalMin, processGenerationIntervalMax));
            }

            while (cpu.IsProcessing);

            cpu.StopProcessing();

            Console.WriteLine(cpuQueue.ProcessedFromQueue1);
            Console.WriteLine(cpuQueue.ProcessedFromQueue2);

            Console.ReadKey();
        }


    }
}