using System.Threading;

namespace Lab4.ReadersWriters
{
	class Reader
	{
		private readonly Storage _storage;

		public Reader(Storage storage)
		{
			_storage = storage;
		}

		public void Start()
		{
			var thread = new Thread(() =>
			{
				string text = _storage.Read();
			});

			thread.Start();
		}
	}
}
