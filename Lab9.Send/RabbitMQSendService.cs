using RabbitMQ.Client;
using System;

namespace Lab9.Send
{
	public sealed class RabbitMQSendService : IDisposable
	{
		private readonly IConnection _connection;
		private readonly IModel _channel;

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
			if (_connection != null)
				_connection.Close();

			if (_channel != null)
				_channel.Close();
		}
	}
}
