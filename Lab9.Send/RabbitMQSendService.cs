using RabbitMQ.Client;
using System;

namespace Lab9.Send
{
	public sealed class RabbitMQSendService : IDisposable
	{
		public RabbitMQSendService()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(
						queue: "lab9",
						durable: false,
						exclusive: false,
						autoDelete: false,
						arguments: null);
		}

		public static RabbitMQSendService Instance { get; } = new RabbitMQSendService();

		private readonly IConnection _connection;
		private readonly IModel _channel;

		public void Send(byte[] body)
		{
			_channel.BasicPublish(
				exchange: "",
				routingKey: "lab9",
				basicProperties: null,
				body: body);
		}

		public void Dispose()
		{
			_connection.Close();
			_channel.Close();
		}
	}
}
