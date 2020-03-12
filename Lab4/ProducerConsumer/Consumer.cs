using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab4.ProducerConsumer
{
	public class Consumer
	{
		private static readonly Random _random = new Random();
		private bool _isInterrupted;
		private readonly string _name;
		private readonly ConcurrentQueue<int> _queue;

		public Consumer(string name, ConcurrentQueue<int> queue)
		{
			_name = name;
			_queue = queue;
		}

		public void Start()
		{
			_isInterrupted = false;

			var thread = new Thread(() =>
			{
				while (!_isInterrupted)
				{
					if (_queue.TryDequeue(out int value))
					{
						Console.WriteLine(_name + " consuming: " + value);
						Thread.Sleep(_random.Next(60, 100));
					}
				}
			});

			thread.Start();
		}

		public void Interrupt()
		{
			_isInterrupted = true;
		}
	}
}