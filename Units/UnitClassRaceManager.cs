using System.Linq;
using Newtonsoft.Json;

namespace Units
{
	/// <summary>
	/// Manages the collection of classes and races
	/// </summary>
	public class UnitClassRaceManager
	{
		/// <summary>
		/// The classes
		/// </summary>
		public readonly UnitClass [] Class;
		/// <summary>
		/// The races
		/// </summary>
		public readonly UnitRace [] Race;

		/// <summary>
		/// Gets the class
		/// </summary>
		/// <param name="className">The name of the class</param>
		public UnitClass GetClass (string className)
		{
			return Class.First (z => z.Name == className);
		}

		/// <summary>
		/// Gets the race
		/// </summary>
		/// <param name="raceName">The name of the race</param>
		public UnitRace GetRace (string raceName)
		{
			return Race.First (z => z.Name == raceName);
		}

		/// <summary>
		/// </summary>
		[JsonConstructor]
		public UnitClassRaceManager (UnitClass [] Class, UnitRace [] Race)
		{
			this.Class = Class;
			this.Race = Race;
		}
	}
	
}