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

		public GemQuantity Clone()
		{
			return (GemQuantity)MemberwiseClone();
		}
	}
}
