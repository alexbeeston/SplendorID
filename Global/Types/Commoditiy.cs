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

		public virtual bool Equals([AllowNull] Commoditiy other)
		{
			return
				Points == Points &&
				Price.Equals(other.Price);
		}
	}
}
