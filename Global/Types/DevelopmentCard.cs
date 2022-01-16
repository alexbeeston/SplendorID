using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Global.Types
{

	public class DevelopmentCard : Commoditiy, IEquatable<DevelopmentCard>
	{
		public DevelopmentLevel Level { get; set; }
		public Gem Gem { get; set; }

		public bool Equals([AllowNull] DevelopmentCard other)
		{
			return
				base.Equals(other) &&
				Level == other.Level &&
				Gem == other.Gem;
		}

		public DevelopmentCard Clone()
		{
			var clone = (DevelopmentCard)MemberwiseClone();
			clone.Price = Price.Clone();
			return clone;
		}
	}
}
