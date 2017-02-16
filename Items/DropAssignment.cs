using System.Collections.Generic;
using System.Linq;
using AoM;
using Helper;

namespace Items
{
	/// <summary>
	/// Assignment from items to weigth. Can create an inventory from that assignemnt.
	/// </summary>
	public class DropAssignment
	{
		readonly Dictionary<string, float> nameDictionary;

		/// <summary>
		/// Creates an <see cref="Inventory"/> from the weigth distribution 
		/// </summary>
		/// <param name="totalDropValue">The expected worth of the full inventory</param>
		public Inventory MakeDrops (float totalDropValue)
		{
			var ret = new Inventory ();
			while (true)
			{
				var posDrops = nameDictionary.Where (z => z.Value <= totalDropValue);
				if (posDrops.Any ())
				{
					var picker = new PickOneProbabilityDictionary<string> (posDrops);
					picker.Normalize ();
					var picked = picker.Pick ();
					var newItem = Program.MyGame.Items.CreateItem<IItem> (picked);
					ret.Add (newItem);
					totalDropValue -= newItem.Value;
				}
				else
					return ret;
			}
		}

		/// <summary>
		/// Merges this assignment with some other <see cref="DropAssignment"/>
		/// </summary>
		public void MergeWith (DropAssignment dropAsg)
		{
			foreach (var x in dropAsg.nameDictionary)
			{
				if (nameDictionary.ContainsKey (x.Key))
					nameDictionary [x.Key] += x.Value;
				else
					nameDictionary.Add (x.Key, x.Value);
			}
		}

		/// <summary>
		/// </summary>
		public DropAssignment ()
		{
			nameDictionary = new Dictionary<string, float> ();
		}

		/// <summary>
		///Initializes a new instance of the <see cref="Items.DropAssignment"/> class. Coyping the values from a dictionary
		/// </summary>
		public DropAssignment (Dictionary<string, float> assignment)
		{
			if (assignment == null)
				throw new System.ArgumentNullException ("assignment");
			nameDictionary = assignment;
		}
	}
}