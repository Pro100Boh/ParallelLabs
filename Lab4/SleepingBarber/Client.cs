using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.SleepingBarber
{
	class Client
	{
		public string Name { get; private set; }

		public Client(string name)
		{
			Name = name;
		}
	}
}
