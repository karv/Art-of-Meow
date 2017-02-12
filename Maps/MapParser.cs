using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Maps
{
	/// <summary>
	/// Provides methods to convert maps into string, and string into maps
	/// </summary>
	public static class MapParser
	{
		/// <summary>
		/// Convert a map into a enumeration os string
		/// </summary>
		public static string[] DataToString (char [,] data)
		{
			var ret = new List<string> ();
			var sizeX = data.GetLength (0);
			var sizeY = data.GetLength (1);

			var line = new StringBuilder ();
			for (int ix = 0; ix < sizeX; ix++)
			{
				for (int iy = 0; iy < sizeY; iy++)
					line.Append (data [ix, iy]);
				ret.Add (line.ToString ());
				line.Clear ();
			}		
			return ret.ToArray ();
		}

		static IEnumerable<char> EnumerateChars (this IEnumerable<IEnumerable<char>> obj)
		{
			foreach (var str in obj)
				foreach (var c in str)
					yield return c;
		}

		/// <summary>
		/// Convert an enumeration of string into the data of a map
		/// </summary>
		public static void StringToData (IEnumerable<string> data, char [,] output)
		{
			StringToData (data.EnumerateChars (), output);
		}

		/// <summary>
		/// Convert an enumeration of char into the data of a map
		/// </summary>
		public static void StringToData (IEnumerable<char> data, char [,] output)
		{
			var i = 0;
			var j = 0;
			try
			{
				var sizeX = output.GetLength (0);
				foreach (var chr in data)
				{
					var ix = i % sizeX;
					var iy = i / sizeX;

					if (Map.ExistSymbol (chr))
					{
						output [ix, iy] = chr;
						i++;
						j++;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine (ex);
			}
		}
	}
	
}