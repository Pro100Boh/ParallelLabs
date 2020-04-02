using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System;
using System.IO;

namespace Lab9.Send.Controllers
{
	[ApiController]
	[Route("api/lab9")]
	public class Lab9Controller : ControllerBase
	{
		private static readonly Random _random = new Random();

		private readonly RabbitMQSendService _sendService;

		public Lab9Controller(RabbitMQSendService sendService)
		{
			_sendService = sendService;
		}

		[HttpGet("gun")]
		[Produces("application/xml")]
		public IActionResult Get()
		{
			return Ok(new Gun
			{
				Model = Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
				Handy = (Handy)_random.Next(0, 2),
				Origin = new[] { "USA", "Germany", "France" }[_random.Next(0, 3)],
				Material = new[] { "Steel", "Aluminum", "Plastic" }[_random.Next(0, 3)],
				ShootingDistance = (ShootingDistance)_random.Next(0, 3),
				EffectiveFiringRange = _random.Next(1, 51) * 100,
				HasClip = _random.Next(0, 2) == 0,
				HasSights = _random.Next(0, 2) == 0
			});
		}

		[HttpPost("gun")]
		[Consumes("application/xml")]
		public IActionResult Post(Gun gun)
		{
			using (var ms = new MemoryStream())
			{
				Serializer.Serialize(ms, gun);
				byte[] body = ms.ToArray();
				_sendService.Send(body);
			}

			return Ok();
		}
	}
}
