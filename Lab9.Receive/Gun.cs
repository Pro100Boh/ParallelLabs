using ProtoBuf;

namespace Lab9.Receive
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

	[ProtoContract]
	public class Gun
	{
		[ProtoMember(1)]
		public string Model { get; set; }

		[ProtoMember(2)]
		public Handy? Handy { get; set; }

		[ProtoMember(3)]
		public string Origin { get; set; }

		[ProtoMember(4)]
		public string Material { get; set; }

		[ProtoMember(5)]
		public ShootingDistance? ShootingDistance { get; set; }

		[ProtoMember(6)]
		public int? EffectiveFiringRange { get; set; }

		[ProtoMember(7)]
		public bool? HasClip { get; set; }

		[ProtoMember(8)]
		public bool? HasSights { get; set; }
	}
}
