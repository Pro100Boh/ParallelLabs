using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab9.Receive
{
	class Program
	{
		public static void Main()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };

			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "lab9",
									 durable: false,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

				var consumer = new EventingBasicConsumer(channel);

				consumer.Received += Receive;

				channel.BasicConsume(queue: "lab9",
									 autoAck: true,
									 consumer: consumer);

				Console.ReadLine();
			}
		}

		static void Receive(object sender, BasicDeliverEventArgs eventArgs)
		{
			var body = eventArgs.Body;

			using (var ms = new MemoryStream(body))
			{
				var gun = (Gun)new BinaryFormatter().Deserialize(ms);

				PrintGunObject(gun);
			}
		}

		static void PrintGunObject(Gun gun)
		{
			Console.WriteLine("Received new object: ");
			Console.WriteLine($"Model: {gun.Model}");
			Console.WriteLine($"Handy: {gun.Handy}");
			Console.WriteLine($"Origin: {gun.Origin}");
			Console.WriteLine($"Material: {gun.Material}");
			Console.WriteLine("TTCs:");
			foreach (var ttc in gun.TTCs)
			{
				Console.WriteLine("\tTTC:");
				Console.WriteLine($"\tShootingDistance: {ttc.ShootingDistance}");
				Console.WriteLine($"\tEffectiveFiringRange: {ttc.EffectiveFiringRange}");
				Console.WriteLine($"\tHasClip: {ttc.HasClip}");
				Console.WriteLine($"\tHasSights: {ttc.HasSights}");
			}
			Console.WriteLine(new string('*', 50));
		}
	}
}

