using System.Collections.Generic;
using System.Linq;
using AoM;
using Helper;

namespace Items
{
	public class DropAssignment
	{
		readonly Dictionary<string, float> nameDictionary;

		public Inventory MakeDrops (float totalDropValue)
		{
			totalDropValue = 100;
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

		public DropAssignment (Dictionary<string, float> assignment)
		{
			if (assignment == null)
				throw new System.ArgumentNullException ("assignment");
			nameDictionary = assignment;
		}
	}
}