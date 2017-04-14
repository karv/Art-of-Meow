using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AoM;
using Cells;
using Debugging;
using Items;
using Newtonsoft.Json;
using Items.Declarations;
using Units.Inteligencia;

namespace Units
{

	/// <summary>
	/// Represents a type o race of unit
	/// </summary>
	public class UnitRace
	{
		/// <summary>
		/// Name of the race
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// The names of the possible classes for this class
		/// </summary>
		public readonly string [] PossibleClasses;

		/// <summary>
		/// Possible classes for this race
		/// </summary>
		public UnitClass [] PossibleClassesRef ()
		{
			var ret = new UnitClass[PossibleClasses.Length];
			for (int i = 0; i < ret.Length; i++)
				ret [i] = Program.MyGame.ClassRaceManager.GetClass (PossibleClasses [i]);
			return ret;
		}

		/// <summary>
		/// Assignment from item name to drop weight.
		/// </summary>
		[JsonIgnore]
		public readonly DropAssignment DropDistribution;

		/// <summary>
		/// Gets the reference to the class in the <see cref="PossibleClasses"/> on an index.
		/// </summary>
		public UnitClass PossibleClass (int i)
		{
			return Program.MyGame.ClassRaceManager.GetClass (PossibleClasses [i]);
		}

		/// <summary>
		/// Attributes distribution for this race
		/// </summary>
		public readonly ReadOnlyDictionary<string, float> AttributesDistribution;
		/// <summary>
		/// Texture used to draw this unit
		/// </summary>
		public readonly string TextureString;

		static readonly Random _r = new Random ();

		/// <summary>
		/// Merges the attributes from a class and a race
		/// </summary>
		public static Dictionary<string,float> mergeAttrDists (UnitRace race, UnitClass cls, float typeWeight = 0.5f)
		{
			if (typeWeight < 0 || typeWeight > 1)
				throw new ArgumentOutOfRangeException (
					"typeWeight",
					"typeWeight must be a non-negative number at most 1");

			var ret = new Dictionary<string,float> ();
			foreach (var assign in race.AttributesDistribution)
				ret.Add (assign.Key, assign.Value * typeWeight);

			float classWeight = 1 - typeWeight;
			foreach (var assign in cls.AttributesDistribution)
			{
				float prevVal;
				if (ret.TryGetValue (assign.Key, out prevVal))
					ret [assign.Key] = prevVal + assign.Value * classWeight;
				else
					ret.Add (assign.Key, assign.Value * classWeight);
			}
			return ret;
		}

		/// <summary>
		/// Makes a new random enemy from this race
		/// </summary>
		/// <param name="grid">The logic grid where this <see cref="Unidad"/> lives</param>
		/// <param name="exp">Experience</param>
		public Unidad MakeEnemy (LogicGrid grid, float exp = 0)
		{
			var ret = new Unidad (grid, TextureString);
			var uClass = PossibleClassesRef () [_r.Next (PossibleClasses.Length)];

			var ai = (uClass.Int as AI)?.Clone () as AI;

			foreach (var x in mergeAttrDists (this, uClass))
				ret.Exp.AddAssignation (x.Key, x.Value);
			ret.Exp.ExperienciaAcumulada = exp;
			ret.Exp.Flush ();

			ret.RecursoHP.Fill ();

			ret.Nombre = Name;

			var dDist = new DropAssignment ();
			dDist.MergeWith (uClass.DropDistribution);
			dDist.MergeWith (DropDistribution);
			ret.Inventory = dDist.MakeDrops (10 + exp);

			foreach (var eq in uClass.StartingEquipment)
			{
				var eqInstance = Program.MyGame.Items.CreateItem (eq);
				var stack = eqInstance as IStackingItem;
				if (stack != null)
					stack.Quantity = 100;
				ret.Inventory.Add (eqInstance);
			}			
			foreach (var i in ret.Inventory.ItemsOfType<IEquipment> ())
				ret.Equipment.EquipItem (i);

			// Invoke this after unit is equiped
			ai.LinkWith (ret);
			DebugAllInfo (ret);
			return ret;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnitRace"/> class.
		/// </summary>
		[JsonConstructor]
		public UnitRace (string Name,
		                 string [] PossibleClasses,
		                 ReadOnlyDictionary<string, float> AttributesDistribution,
		                 Dictionary<string, float> DropDistribution,
		                 string TextureString)
		{
			this.Name = Name;
			this.PossibleClasses = PossibleClasses;
			this.AttributesDistribution = AttributesDistribution;
			this.TextureString = TextureString;
			this.DropDistribution = new DropAssignment (DropDistribution);
		}

		/// <summary>
		/// Muestra toda la información de una unidad por el debug
		/// </summary>
		/// <param name="u">Unidad</param>
		[Conditional ("DEBUG")]
		public static void DebugAllInfo (Unidad u)
		{
			Debug.WriteLine (
				string.Format ("Unidad creada: {0}", u.Nombre),
				DebugCategories.UnitFactory);
			foreach (var re in u.Recursos.Enumerate ())
			{
				foreach (var pa in re.EnumerateParameters ())
				{
					Debug.Write (
						string.Format (
							"{2}.{0}: {1}\t",
							pa.NombreÚnico,
							pa.Valor,
							re.NombreÚnico),
						DebugCategories.UnitFactory);
				}
				Debug.WriteLine ("", DebugCategories.UnitFactory);
			}
		}
	}
}