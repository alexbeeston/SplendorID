using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Global.Types
{
	public class GemQuantity : IEquatable<GemQuantity>
	{
		public int Diamond { get; set; }
		public int Emerald { get; set; }
		public int Onyx { get; set; }
		public int Ruby { get; set; }
		public int Sapphire { get; set; }

		public bool Equals([AllowNull] GemQuantity other)
		{
			return
				Emerald == other.Emerald &&
				Diamond == other.Diamond &&
				Onyx == other.Onyx &&
				Ruby == other.Ruby &&
				Sapphire == other.Sapphire;
		}

		/// <summary>
		/// Constructor used for Newton soft
		/// </summary>
		public GemQuantity() { }

		public GemQuantity(int numPlayers)
		{
			var numTokens = numPlayers switch
			{
				2 => 4,
				3 => 5,
				4 => 7,
				_ => throw new Exception("Only two, three, or four players may play Splendor"),
			};
			Diamond = numTokens;
			Emerald = numTokens;
			Onyx = numTokens;
			Ruby = numTokens;
			Sapphire = numTokens;
		}

		public GemQuantity Clone()
		{
			return (GemQuantity)MemberwiseClone();
		}
	}
}
