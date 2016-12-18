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
				if (rnd.NextDouble () < item.Weight)
					ret.Add (item.Item);
			
			return ret;
		}

		/// <summary>
		/// Add the specified obj and chance.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The chance is a number between 0 and 1 (inclusive)</exception>
		public void Add (TItem obj, float chance)
		{
			if (chance < 0 || chance > 1)
				throw new InvalidOperationException ();
			
			probItems.Add (new ItemWeightPair<TItem> (obj, chance));
		}

		/// <summary>
		/// </summary>
		public ProbabilityInstanceSet ()
		{
			probItems = new List<ItemWeightPair<TItem>> ();
		}
	}
	
}