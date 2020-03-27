using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lab8
{
	public enum Handy
	{
		OneHanded,
		TwoHanded,
	}

	public enum ShootingDistance
	{
		Short,
		Middle,
		Long
	}

	public class TTC
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ShootingDistance ShootingDistance { get; set; }

		public int EffectiveFiringRange { get; set; }

		public bool HasClip { get; set; }

		public bool HasSights { get; set; }
	}

	public class Gun
	{
		public string Model { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public Handy Handy { get; set; }

		public string Origin { get; set; }

		public string Material { get; set; }

		public List<TTC> TTCs { get; set; }
	}
}