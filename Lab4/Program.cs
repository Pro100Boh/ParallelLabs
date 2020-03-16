using Lab4.DiningPhilosophers;
using Lab4.ProducerConsumer;
using Lab4.ReadersWriters;
using Lab4.SleepingBarber;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab4
{
	class Program
	{
		static void Main(string[] args)
		{
			SimulateReadersWriters();
		}

		public static void SimulateProducerConsumer()
		{
			var queue = new ConcurrentQueue<int>();

			var producers = new List<Producer>();
			var consumers = new List<Consumer>();

			for (int i = 0; i < 2; i++)
			{
				producers.Add(new Producer($"Producer-{i}", queue));
			}

			for (int i = 0; i < 3; i++)
			{
				consumers.Add(new Consumer($"Consumer-{i}", queue));
			}

			foreach (var producer in producers)
			{
				producer.Start();
			}

			foreach (var consumer in consumers)
			{
				consumer.Start();
			}

			while (!queue.IsEmpty || producers.Any(p => p.Producing)) ;

			foreach (var consumer in consumers)
			{
				consumer.Interrupt();
			}

		}

		public static void SimulateDiningPhilosophers()
		{
			var philosophers = new Philosopher[5];
			for (int i = 0; i < philosophers.Length; i++)
			{
				philosophers[i] = new Philosopher(philosophers, i);
			}

			foreach (var philosopher in philosophers)
			{
				philosopher.LeftFork = philosopher.LeftPhilosopher.RightFork ?? new Semaphore(1, 1);
				philosopher.RightFork = philosopher.RightPhilosopher.LeftFork ?? new Semaphore(1, 1);
			}

			var philosopherThreads = new Thread[philosophers.Length];

			for (int i = 0; i < philosophers.Length; i++)
			{
				philosopherThreads[i] = new Thread(philosophers[i].EatAll);
				philosopherThreads[i].Start();
			}

			foreach (var thread in philosopherThreads)
			{
				thread.Join();
			}
		}

		public static void SimulateSleepingBarber()
		{
			var random = new Random();
			var queue = new ConcurrentQueue<Client>();
			var barber = new Barber(queue);
			var thread = new Thread(barber.CutHair);
			thread.Start();

			for (int i = 0; i < 50; i++)
			{
				Thread.Sleep(random.Next(0, 100));
				queue.Enqueue(new Client($"client #{i}"));
			}

			thread.Join();
		}

		public static void SimulateReadersWriters()
		{
			var storage = new Storage();

			for (int i = 0; i < 256; ++i)
			{
				if (i % 50 == 0)
				{
					new Writer(storage).Start();
				}
				else
				{
					new Reader(storage).Start();
				}
			}
		}
	}
}
