using System;
using AoM;
using Newtonsoft.Json;
using Units;

namespace Maps
{
	/// <summary>
	/// Represents a single population rule
	/// </summary>
	public class PopulationRule
	{
		/// <summary>
		/// This is requiered to have access to other rules, so these rules are not forceful independent
		/// </summary>
		[JsonIgnore]
		public Populator Populator { get; private set; }

		/// <summary>
		/// Gets the chance of run this population rule
		/// </summary>
		/// <value>The chance.</value>
		public float Chance { get; set; }

		/// <summary>
		/// The stacks created by this rule
		/// </summary>
		public DistributedStack [] Stacks;

		/// <summary>
		/// Links this rule to a <see cref="Populator"/>
		/// </summary>
		public void LinkWith (Populator populator)
		{
			if (populator == null)
				throw new ArgumentNullException ("populator");
			if (Populator != null)
				throw new InvalidOperationException ("This rule already has a populator");
			
			Populator = populator;
		}
	}

	/// <summary>
	/// Represents a pain Race-distribution
	/// </summary>
	public class DistributedStack
	{
		/// <summary>
		/// The name of the race
		/// </summary>
		public string RaceName;

		/// <summary>
		/// Gets the race using <see cref="Juego.ClassRaceManager"/>
		/// </summary>
		[JsonIgnore]
		public UnitRace Race
		{
			get
			{
				return Program.MyGame.ClassRaceManager.GetRace (RaceName);
			}
		}

		/// <summary>
		/// The distribution on the unit's quantity
		/// </summary>
		public float SpawnProbPerCell;
	}
}