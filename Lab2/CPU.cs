using System;
using System.Threading;

namespace Lab2
{
    public class CPU
    {
        private readonly Random _random = new Random();

        private readonly int _minTimeOfProcess;

        private readonly int _maxTimeOfProcess;

        private readonly ICPUQueue _queue;

        private bool _canProcess = true;

        public bool IsProcessing { get; private set; }

        public CPU(int minTimeOfProcess, int maxTimeOfProcess, ICPUQueue queue)
        {
            _minTimeOfProcess = minTimeOfProcess;
            _maxTimeOfProcess = maxTimeOfProcess;
            _queue = queue;
        }

        public void StartProcessing()
        {
            while (_canProcess)
            {
                CPUProcess process = _queue.Get();

                if (process != null)
                {
                    IsProcessing = true;
                    Thread.Sleep(_random.Next(_minTimeOfProcess, _maxTimeOfProcess + 1));
                }
                else
                {
                    IsProcessing = false;
                }
            }
        }

        public void StopProcessing()
        {
            _canProcess = false;
        }

        public void AddProcessToQueue(CPUProcess process)
        {
            _queue.Put(process);
        }
    }
}