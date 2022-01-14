using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Types
{
	public abstract class Commoditiy
	{
		public int Points { get; set; }
		public GemQuantity Price { get; set; }
		public string Id { get; set; }
	}
}
