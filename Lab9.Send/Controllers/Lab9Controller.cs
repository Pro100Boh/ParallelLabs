using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
		public IActionResult Get()
		{
			return Ok(new Gun
			{
				Model = "a1b2c3",
				Handy = Handy.TwoHanded,
				Origin = "USA",
				Material = "Steel",
				TTCs = new List<TTC>
				{
					new TTC
					{
						ShootingDistance = ShootingDistance.Middle,
						EffectiveFiringRange = 1000,
						HasClip = false,
						HasSights = false
					}
				}

			});
		}

		[HttpPost("gun")]
		public IActionResult Post(Gun gun)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			using (var ms = new MemoryStream())
			{
				new BinaryFormatter().Serialize(ms, gun);
				_sendService.Send(ms.ToArray());
			}

			return Ok();

		}
	}
}
