using System;
using Maps;
using NUnit.Framework;

namespace Test
{
	[TestFixtureAttribute]
	public class MapReadingTest
	{
		[Test]
		public void ReadMaps ()
		{
			var files = System.IO.Directory.EnumerateFiles (Map.MapDir, "*.map");
			foreach (var file in files) {
				Console.WriteLine ("Testing " + file);
				var map = Map.ReadFromFile (file);
				map.GenerateGrid (0);
			}
		}
		
	}
}