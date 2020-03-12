using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Lab4.ProducerConsumer
{
	public class Producer
	{
		private static readonly Random _random = new Random();
		private readonly string _name;
		private readonly ConcurrentQueue<int> _queue;

		public bool Producing { get; private set; }

		public Producer(string name, ConcurrentQueue<int> queue)
		{
			_name = name;
			_queue = queue;
		}

		public void Start()
		{
			Producing = true;

			var thread = new Thread(() =>
			{
				for (int i = 0; i < 50; ++i)
				{
					int value = _random.Next(10_000);
					Console.WriteLine(_name + " produced: " + value);

					_queue.Enqueue(value);
					Thread.Sleep(_random.Next(40, 60));
				}

				Producing = false;
			}
			);

			thread.Start();
		}
	}
}