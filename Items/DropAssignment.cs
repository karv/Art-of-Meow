﻿using System;
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
		const float probAddMod = 0.20f;
		const int maxDropsCount = 5;
		static Random _r = new Random ();
		readonly Dictionary<string, float> nameDictionary;

		/// <summary>
		/// Creates an <see cref="Inventory"/> from the weigth distribution 
		/// </summary>
		/// <param name="totalDropValue">The expected worth of the full inventory</param>
		public Inventory MakeDrops (float totalDropValue)
		{
			var ret = new Inventory ();
			for (int c = 0; c < maxDropsCount; c++)
			{
				var posDrops = nameDictionary.Where (z => z.Value <= totalDropValue);
				if (posDrops.Any ())
				{
					var picker = new PickOneProbabilityDictionary<string> (posDrops);
					picker.Normalize ();
					var picked = picker.Pick ();
					var newItem = Program.MyGame.Items.CreateItem<IItem> (picked);
					var stackItem = newItem as IStackingItem;
					if (stackItem != null)
					{
						// Requiered to calculate the items value properly
						stackItem.Quantity = 1; 

						// Max stack size = 100
						var maxStack = (int)Math.Min (totalDropValue / stackItem.Value, 100);
						stackItem.Quantity = _r.Next (1, maxStack);
					}
					if (newItem.AllowedMods.Length > 0)
						// Add mods
						while (_r.NextDouble () < probAddMod)
						{
							// Add a new item modification
							var newMod = newItem.AllowedMods [_r.Next (newItem.AllowedMods.Length)];

							newItem.Modifiers.Modifiers.Add (newMod);
						}
					ret.Add (newItem);
					totalDropValue -= newItem.Value;
				}
			}
			return ret;
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
				throw new ArgumentNullException ("assignment");
			nameDictionary = assignment;
		}
	}
}