using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Helper
{
	/// <summary>
	/// Provides methods to pick randomly a subset if a list
	/// </summary>
	public class ProbabilityInstanceSet <TItem> : IDistribution<ICollection<TItem>>
	{
		[JsonProperty (PropertyName = "Values")]
		readonly List<ItemWeightPair<TItem>> probItems;

		readonly Random rnd = new Random ();

		/// <summary>
		/// Picks an item in the distribution
		/// </summary>
		public ICollection<TItem> Pick ()
		{
			var ret = new List<TItem> ();

			foreach (var item in probItems)
				if (item.Weight < rnd.NextDouble ())
					ret.Add (item.Item);
			
			return ret;
		}

		/// <summary>
		/// </summary>
		public ProbabilityInstanceSet ()
		{
			probItems = new List<ItemWeightPair<TItem>> ();
		}
	}
	
}