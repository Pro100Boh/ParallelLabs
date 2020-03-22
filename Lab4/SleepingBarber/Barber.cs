using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lab4.SleepingBarber
{
	class Barber
	{
		private static readonly Random _random = new Random();
		private readonly BarbershopQueue _queue;

		public Barber(BarbershopQueue queue)
		{
			_queue = queue;
		}

		public void CutHair()
		{
			while (!_queue.IsEmpty)
			{
				_queue.TryGet(out var client);

				Console.WriteLine($"Started hair cutting to {client.Name}");

				Thread.Sleep(_random.Next(40, 60));

				Console.WriteLine($"Done hair cutting to {client.Name}");
			}

			GoToSleep();
		}

		private void GoToSleep()
		{
			Console.WriteLine("Barber is sleeping...");

			while (_queue.IsEmpty);

			Console.WriteLine("Barber waking up - customer arrived!");

			CutHair();
		}
	}
}