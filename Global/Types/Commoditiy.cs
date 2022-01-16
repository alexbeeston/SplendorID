using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Global.Types
{
	public abstract class Commoditiy : IEquatable<Commoditiy>
	{
		public int Points { get; set; }
		public GemQuantity Price { get; set; }

		public bool Equals([AllowNull] Commoditiy other)
		{
			return
				Points == other.Points &&
				Price.Equals(other.Price);
		}
	}
}
