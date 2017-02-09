using System.Linq;
using Newtonsoft.Json;

namespace Units
{
	public class UnitClassRaceManager
	{
		public readonly UnitClass [] Class;
		public readonly UnitRace [] Race;

		public UnitClass GetClass (string className)
		{
			return Class.First (z => z.Name == className);
		}

		public UnitRace GetRace (string raceNAme)
		{
			return Race.First (z => z.Name == raceNAme);
		}

		[JsonConstructor]
		public UnitClassRaceManager (UnitClass [] Class, UnitRace [] Race)
		{
			this.Class = Class;
			this.Race = Race;
		}
	}
	
}