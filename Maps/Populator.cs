using System;
using System.Collections.Generic;
using System.Linq;
using Cells;
using Units;
using Newtonsoft.Json;

namespace Maps
{
	/// <summary>
	/// Provides methods to populate and repopulate a map.
	/// </summary>
	public class Populator
	{
		[JsonProperty]
		internal readonly PopulationRule [] Rules;
		[JsonIgnore]
		static readonly Random _r = new Random ();

		/// <summary>
		/// Gets a list of <see cref="Unidad"/> whose elements are new units.
		/// </summary>
		/// <param name="grid">Grid where to place them</param>
		/// <param name="totalExp">Combined experience</param>
		public IList<Unidad> BuildPop (LogicGrid grid, float totalExp = 0)
		{
			var ret = new List<Unidad> ();
			foreach (var rule in Rules.Where (rule => _r.NextDouble () < rule.Chance))
				foreach (var st in rule.Stacks)
				{
					var qt = st.UnitQuantityDistribution.Pick ();
					// build 'qt' copies of this race
					for (int i = 0; i < qt; i++)
					{
						// TODO: calculate properly the exp of this unit
						var un = st.Race.MakeEnemy (grid, totalExp);
						ret.Add (un);
					}
				}
			return ret;
		}

		void linkRules ()
		{
			foreach (var r in Rules)
				r.LinkWith (this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Maps.Populator"/> class.
		/// </summary>
		/// <param name="Rules">A set of generation rules</param>
		[JsonConstructor]
		public Populator (PopulationRule [] Rules)
		{
			this.Rules = Rules;
			linkRules ();
		}
	}
}