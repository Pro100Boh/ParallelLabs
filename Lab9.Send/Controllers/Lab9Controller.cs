using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.IO;

namespace Lab9.Send.Controllers
{
	[ApiController]
	[Route("api/lab9")]
	public class Lab9Controller : ControllerBase
	{
		private RabbitMQSendService _sendService;

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
				Model = "a1b2c3",
				Handy = Handy.TwoHanded,
				Origin = "USA",
				Material = "Steel",
				ShootingDistance = ShootingDistance.Middle,
				EffectiveFiringRange = 1000,
				HasClip = false,
				HasSights = false
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
