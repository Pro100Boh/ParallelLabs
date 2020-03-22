using System;
using System.Collections.Generic;

namespace Lab4.SleepingBarber
{
	public class BarbershopQueue
	{
		private readonly Queue<Client> _queue = new Queue<Client>();

		private readonly object _lock = new object();

		private readonly int _maxQueueLength;

		public bool IsEmpty => _queue.Count == 0;

		public BarbershopQueue(int maxQueueLength)
		{
			_maxQueueLength = maxQueueLength;
		}

		public bool TryGet(out Client client)
		{
			lock (_lock)
			{
				return _queue.TryDequeue(out client);
			}
		}

		public void Put(Client client)
		{
			lock (_lock)
			{
				if (_queue.Count >= _maxQueueLength)
				{
					Console.WriteLine($"Client {client} left");
					return;
				}

				_queue.Enqueue(client);
			}
		}
	}
}
