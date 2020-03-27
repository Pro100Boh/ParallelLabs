using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lab7.Server
{
	class Program
	{
		private static readonly List<Socket> _sockets = new List<Socket>();

		private static bool _runServer = true;

		private static string TimeNowFormatted => DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss");

		static void Main()
		{
			var serverThread = Task.Run(StartServer);

			string command = null;

			while (true)
			{
				Console.Write(">");
				command = Console.ReadLine();

				if (string.Equals(command, "exit", StringComparison.OrdinalIgnoreCase))
				{
					_runServer = false;
					break;
				}
				else if (string.Equals(command, "list", StringComparison.OrdinalIgnoreCase))
				{
					UpdateSocketsList();
					foreach (var socket in _sockets)
					{
						Console.WriteLine(socket.RemoteEndPoint.ToString());
					}
				}
				else if (string.Equals(command, "send", StringComparison.OrdinalIgnoreCase))
				{
					Console.Write("Address: ");
					string address = Console.ReadLine().Trim();
					UpdateSocketsList();
					var socket = _sockets.FirstOrDefault(s => s.RemoteEndPoint.ToString() == address);
					if (socket == default)
					{
						Console.WriteLine("Wrong address!");
						continue;
					}

					Console.Write("Message: ");
					string message = Console.ReadLine();

					Console.WriteLine("Time delay: ");
					if (!TimeSpan.TryParse(Console.ReadLine(), out var delay))
					{
						Console.WriteLine("Wrong time format!");
						continue;
					}
					Console.WriteLine($"[{TimeNowFormatted}] Message will be sent, delay = {delay}");
					SendMessageAtSpecificTime(delay, socket, message);
				}
				else
				{
					Console.WriteLine("Unknown command!");
				}
			}

			foreach (var socket in _sockets)
			{
				socket.Shutdown(SocketShutdown.Both);
				socket.Close();
			}

			Console.ReadKey();
		}

		static void SendMessageAtSpecificTime(TimeSpan delay, Socket socket, string message)
		{
			var data = Encoding.Unicode.GetBytes(message);

			Task.Delay(delay).ContinueWith((x) =>
			{
				try
				{
					socket.Send(data);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(ex.Message);
					Console.ForegroundColor = ConsoleColor.White;
				}
			});
		}

		static void UpdateSocketsList()
		{
			lock (_sockets)
			{
				_sockets.RemoveAll(s => !s.Connected);
			}
		}

		static void StartServer()
		{
			var ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);

			var listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				listenSocket.Bind(ipPoint);
				listenSocket.Listen(16);

				while (_runServer)
				{
					Socket socket = listenSocket.Accept();

					lock (_sockets)
					{
						_sockets.Add(socket);
					}
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.White;
			}
		}
	}
}
