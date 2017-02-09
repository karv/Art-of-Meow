using System;
using System.Linq;
using Helper;
using Newtonsoft.Json;
using Units;
using AoM;

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

		public void LinkWith (Populator populator)
		{
			if (populator == null)
				throw new ArgumentNullException ("populator");
			if (Populator != null)
				throw new InvalidOperationException ("This rule already has a populator");
			
			Populator = populator;
		}
	}

	public class DistributedStack
	{
		public string RaceName;

		public UnitRace Race
		{
			get
			{
				return Program.MyGame.ClassRaceManager.GetRace (RaceName);
			}
		}

		public IDistribution<int> UnitQuantityDistribution;
	}
}