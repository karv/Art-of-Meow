using System;
using NUnit.Framework;

namespace Test
{
	[TestFixtureAttribute]
	public class MapReadingTest
	{
		[Test]
		public void ReadMaps ()
		{
			var files = System.IO.Directory.EnumerateFiles (AoM.FileNames.MapFolder, "*.json");
			foreach (var file in files) {
				Console.WriteLine ("Testing " + file);
				var map = Maps.Map.ReadFromFile (file);
				map.GenerateGrid (0);
			}
		}
		
	}
}