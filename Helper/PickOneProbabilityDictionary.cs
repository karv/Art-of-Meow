using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Helper
{
	/// <summary>
	/// Represents a dicionary of probabilities
	/// </summary>
	public class PickOneProbabilityDictionary<TItem> : IDistribution<TItem>
	{
		[JsonProperty (PropertyName = "Values")]
		readonly List<ItemWeightPair<TItem>> support;

		/// <summary>
		/// Adds an entry to the list
		/// </summary>
		public void Add (TItem item, float weight)
		{
			if (weight == 0)
				return;
			support.Add (new ItemWeightPair<TItem> (item, weight));
			OnDataChange ();
		}

		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Remove (TItem item)
		{
			var comparer = EqualityComparer<TItem>.Default;
			var itemHash = comparer.GetHashCode (item);
			support.RemoveAll (z => itemHash == comparer.GetHashCode (z.Item) &&
			comparer.Equals (item, z.Item));
			OnDataChange ();
		}

		/// <summary>
		/// Should invoke when the data is changed
		/// </summary>
		protected void OnDataChange ()
		{
			IsNormalized = false;
		}

		/// <summary>
		/// Gets a value indicating if this class is normalized
		/// </summary>
		public bool IsNormalized { get; private set; }

		/// <summary>
		/// A random generator
		/// </summary>
		protected Random Rnd = new Random ();

		/// <summary>
		/// Picks an item
		/// </summary>
		public TItem Pick ()
		{
			if (!IsNormalized)
				throw new InvalidOperationException ("This instance is not normalized.\nInvoke parameterless Normalize method");
			if (!HasWeight ())
				throw new InvalidCastException ();

			var chance = Rnd.NextDouble ();
			var currIndex = 0;
			while (chance > support [currIndex].Weight)
			{
				chance -= support [currIndex].Weight;
				currIndex++;
			}
			return support [currIndex].Item;
		}

		/// <summary>
		/// Gets the weight sum.
		/// </summary>
		[JsonIgnore]
		public float WeightSum
		{
			get { return support.Sum (z => z.Weight); }
		}

		/// <summary>
		/// Determines if this class has positive weight
		/// </summary>
		public bool HasWeight ()
		{
			return support.Any (); //
		}

		/// <summary>
		/// Finds the pair by item.
		/// </summary>
		/// <returns>The pair by item.</returns>
		protected ItemWeightPair<TItem> FindPairByItem (TItem item)
		{
			var comparer = EqualityComparer<TItem>.Default;
			var itemHash = comparer.GetHashCode (item);
			return support.Find (z => comparer.GetHashCode (z.Item) == itemHash &&
			comparer.Equals (z.Item, item));
		}

		/// <summary>
		/// Gets or set the weight in a given item
		/// </summary>
		/// <param name="item">Item.</param>
		public float this [TItem item]
		{
			get { return FindPairByItem (item).Weight; }
			set
			{
				if (value == 0)
					Remove (item);
				FindPairByItem (item).Weight = value;
				OnDataChange ();
			}
		}

		/// <summary>
		/// Normalize the sum
		/// </summary>
		public void Normalize ()
		{
			var oldWeight = WeightSum;
			if (oldWeight == 0)
				throw new InvalidOperationException ("Cannot normanize vector zero.");
			
			var factor = 1 / oldWeight;

			foreach (var item in support)
				item.Weight *= factor;
			IsNormalized = true;
		}

		/// <summary>
		/// Normalize the sum
		/// </summary>
		/// <param name="newWeight">New weight.</param>
		public void Normalize (float newWeight)
		{
			if (newWeight < 0)
				throw new InvalidCastException ();

			if (newWeight == 0)
				support.Clear ();
			
			Normalize ();
			foreach (var x in support)
				x.Weight *= newWeight;
			OnDataChange ();
		}

		/// <summary>
		/// </summary>
		/// <param name="values">Values.</param>
		[JsonConstructor]
		public PickOneProbabilityDictionary (ICollection<ItemWeightPair<TItem>> values)
		{
			support = new List<ItemWeightPair<TItem>> (values);
		}

		/// <summary>
		/// </summary>
		public PickOneProbabilityDictionary ()
		{
			support = new List<ItemWeightPair<TItem>> ();
		}
	}
}