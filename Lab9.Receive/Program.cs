using Dapper;
using Microsoft.Data.SqlClient;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Data;
using System.IO;

namespace Lab9.Receive
{
	class Program
	{
		private const string _connectionString = "Data Source=DESKTOP-MD9SQKS;Initial Catalog=lab9db;Integrated Security=True";

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
			try
			{
				byte[] body = eventArgs.Body;

				using (var ms = new MemoryStream(body))
				{
					var gun = Serializer.Deserialize<Gun>(ms);

					Console.WriteLine("Received new object: ");

					PrintGunObject(gun);

					using (IDbConnection db = new SqlConnection(_connectionString))
					{
						string sqlQuery = "INSERT INTO Guns (Model, Handy, Origin, Material, ShootingDistance, EffectiveFiringRange, HasClip, HasSights) " +
							"VALUES(@Model, @Handy, @Origin, @Material, @ShootingDistance, @EffectiveFiringRange, @HasClip, @HasSights)";

						db.Execute(sqlQuery, gun);
					}

					Console.WriteLine("\nObject saved in Db");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			Console.WriteLine(new string('*', 30));
		}

		static void PrintGunObject(Gun gun)
		{
			Console.WriteLine($"Model: {gun.Model}");
			Console.WriteLine($"Handy: {gun.Handy}");
			Console.WriteLine($"Origin: {gun.Origin}");
			Console.WriteLine($"Material: {gun.Material}");
			Console.WriteLine($"ShootingDistance: {gun.ShootingDistance}");
			Console.WriteLine($"EffectiveFiringRange: {gun.EffectiveFiringRange}");
			Console.WriteLine($"HasClip: {gun.HasClip}");
			Console.WriteLine($"HasSights: {gun.HasSights}");
		}
	}
}

