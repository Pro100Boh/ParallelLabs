using System.Collections.Generic;

namespace Lab2
{
    public class CPUQueue : ICPUQueue
    {
        private readonly Queue<CPUProcess> _internalQueue1 = new Queue<CPUProcess>();

        private readonly Queue<CPUProcess> _internalQueue2 = new Queue<CPUProcess>();

        public int ProcessedFromQueue1 { get; private set; } = 0;
        public int ProcessedFromQueue2 { get; private set; } = 0;

        public int Length => _internalQueue1.Count;

        public CPUProcess Get()
        {
            lock (this)
            {
                if (_internalQueue1.Count > _internalQueue2.Count)
                {
                    ProcessedFromQueue1++;
                    return _internalQueue1.Dequeue();
                }
                else if (_internalQueue2.Count > 0)
                {
                    ProcessedFromQueue2++;
                    return _internalQueue2.Dequeue();
                }

                return null;
            }
        }

        public void Put(CPUProcess process)
        {
            lock (this)
            {
                if (_internalQueue1.Count > _internalQueue2.Count)
                    _internalQueue2.Enqueue(process);
                else
                    _internalQueue1.Enqueue(process);
            }
        }
    }
}