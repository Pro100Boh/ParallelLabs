using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lab9.Send
{
	[Serializable]
	public enum Handy
	{
		OneHanded,
		TwoHanded,
	}

	[Serializable]
	public enum ShootingDistance
	{
		Short,
		Middle,
		Long
	}

	[ProtoContract]
	public class Gun
	{
		[Required]
		[ProtoMember(1)]
		public string Model { get; set; }

		[Required]
		[ProtoMember(2)]
		public Handy? Handy { get; set; }

		[Required]
		[ProtoMember(3)]
		public string Origin { get; set; }

		[Required]
		[ProtoMember(4)]
		public string Material { get; set; }

		[Required]
		[ProtoMember(5)]
		public ShootingDistance? ShootingDistance { get; set; }

		[Required, Range(100, 5000)]
		[ProtoMember(6)]
		public int? EffectiveFiringRange { get; set; }

		[Required]
		[ProtoMember(7)]
		public bool? HasClip { get; set; }

		[Required]
		[ProtoMember(8)]
		public bool? HasSights { get; set; }
	}
}
