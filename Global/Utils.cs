using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Global.Types;

namespace Global
{
	public static class Utils
	{
		public static List<DevelopmentCard> ReadAllDevelopmentCards(string relativePathToDataDirectory)
		{
			return JsonConvert.DeserializeObject<List<DevelopmentCard>>(File.ReadAllText(Path.Join(relativePathToDataDirectory, "DevelopmentCards.json")));
		}
	}
}
