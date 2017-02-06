using System;

namespace Helper
{
	/// <summary>
	/// Represents a pair of item-weight
	/// </summary>
	public class ItemWeightPair<TItem>
	{
		/// <summary>
		/// The item
		/// </summary>
		public readonly TItem Item;

		float weight;

		/// <summary>
		/// Gets or sets the weight
		/// </summary>
		/// <value>The weight.</value>
		public float Weight
		{
			get
			{
				return weight;
			}
			set
			{
				if (value <= 0)
					throw new InvalidOperationException ();
				weight = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="weight">Weight.</param>
		public ItemWeightPair (TItem item, float weight)
		{
			Item = item;
			Weight = weight;
		}
	}
}