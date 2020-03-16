using System;
using System.Threading;

namespace Lab4.ReadersWriters
{
	public class Writer
	{
		private readonly Storage _storage;

		public Writer(Storage storage)
		{
			_storage = storage;
		}

		public void Start()
		{
			var thread = new Thread(() =>
			{
				_storage.Write(Guid.NewGuid().ToString());

			});

			thread.Start();
		}
	}
}
