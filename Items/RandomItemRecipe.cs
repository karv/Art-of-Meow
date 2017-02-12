using System;
using AoM;
using Newtonsoft.Json;

namespace Items
{
	/// <summary>
	/// Creates items from a list of allowed types
	/// </summary>
	public class RandomItemRecipe : IItemFactory
	{
		readonly static Random _r = new Random ();
		// TODO: ItemVal not impemented
		/// <summary>
		/// Min item value
		/// </summary>
		public readonly float MinItemVal;
		/// <summary>
		/// Max item value
		/// </summary>
		public readonly float MaxItemVal;
		/// <summary>
		/// Allowed types
		/// </summary>
		public readonly string [] AllowedItemNames;

		/// <summary>
		/// Creates an item
		/// </summary>
		public IItem Create ()
		{
			var type = AllowedItemNames [_r.Next (AllowedItemNames.Length)];
			return Program.MyGame.Items.CreateItem (type);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.RandomItemRecipe"/> class.
		/// </summary>
		public RandomItemRecipe ()
		{
			MinItemVal = 0;
			MaxItemVal = float.PositiveInfinity;
			AllowedItemNames = new [] { "Cuchillo", "Lanza" };
		}

		[JsonConstructor]
		RandomItemRecipe (float MinItemVal, float MaxItemVal, string [] AllowedItemNames)
		{
			if (AllowedItemNames == null)
				throw new ArgumentNullException (nameof (AllowedItemNames));
			this.AllowedItemNames = AllowedItemNames;
			this.MinItemVal = MinItemVal;
			this.MaxItemVal = MaxItemVal;
		}
	}
}