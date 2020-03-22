using System;
using System.Threading;

namespace Lab4.ReadersWriters
{
	public class Storage
	{
		private readonly Random _random;
		private readonly Semaphore _semaphore;
		private int _readersCount;
		private string _value;

		public Storage()
		{
			_random = new Random();
			_semaphore = new Semaphore(1, 1);
			_value = null;
		}

		public string Read()
		{
			if (_readersCount == 0)
			{
				_semaphore.WaitOne();
			}

			Interlocked.Increment(ref _readersCount);

			Console.WriteLine("reading");
			Thread.Sleep(_random.Next(5, 15));
			string result = _value;

			Interlocked.Decrement(ref _readersCount);

			if (_readersCount == 0)
			{
				_semaphore.Release();
			}

			return result;
		}

		public void Write(string value)
		{
			_semaphore.WaitOne();

			Console.WriteLine("writing");
			Thread.Sleep(_random.Next(2000, 3000));
			_value = value;

			_semaphore.Release();
		}
	}
}
