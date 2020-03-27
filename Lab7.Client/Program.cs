using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab7.Client
{
	class Program
	{
		private static string TimeNowFormatted => DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss");

		static void Main(string[] args)
		{
			try
			{
				var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);

				var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				
				socket.Connect(ipPoint);

				while (true)
				{
					var buffer = new byte[256];
					var builder = new StringBuilder();
					int bytes = 0;

					bytes = socket.Receive(buffer, buffer.Length, 0);
					builder.Append(Encoding.Unicode.GetString(buffer, 0, bytes));
					Console.WriteLine($"[{TimeNowFormatted}] New message: >> {builder.ToString()}");
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ReadKey();
			}
		}
	}
}
