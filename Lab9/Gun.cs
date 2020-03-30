using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Lab9
{
	public enum Handy
	{
		[EnumMember(Value = "OneHanded")]
		OneHanded,
		[EnumMember(Value = "TwoHanded")]
		TwoHanded,
	}

	public enum ShootingDistance
	{
		[EnumMember(Value = "Short")]
		Short,
		[EnumMember(Value = "Middle")]
		Middle,
		[EnumMember(Value = "Long")]
		Long
	}

	[Serializable]
	public class TTC
	{
		[Required]
		[Range(0, 2)]
		public ShootingDistance? ShootingDistance { get; set; }

		[Required]
		[Range(100, 5000)]
		public int EffectiveFiringRange { get; set; }

		[Required]
		public bool HasClip { get; set; }

		[Required]
		public bool HasSights { get; set; }
	}

	[Serializable]
	public class Gun
	{
		[Required]
		public string Model { get; set; }

		[Required]
		[Range(0, 1)]
		public Handy? Handy { get; set; }

		[Required]
		public string Origin { get; set; }

		[Required]
		public string Material { get; set; }

		[Required]
		public List<TTC> TTCs { get; set; }
	}
}
