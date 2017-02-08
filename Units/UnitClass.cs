using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Cells;
using Debugging;
using Newtonsoft.Json;

namespace Units
{
	public class UnitClass
	{
		public readonly string Name;
		public readonly ReadOnlyDictionary<string, float> AttributesDistribution;

		[JsonConstructor]
		public UnitClass (string Name, Dictionary<string, float> AttributesDistribution)
		{
			this.Name = Name;
			this.AttributesDistribution = new ReadOnlyDictionary<string, float> (AttributesDistribution);
		}
		
	}

	public class UnitRace
	{
		public readonly string Name;
		public readonly UnitClass [] PossibleClasses;
		public readonly ReadOnlyDictionary<string, float> AttributesDistribution;
		public readonly string TextureString;
		public static readonly Random _r = new Random ();

		static Dictionary<string,float> mergeAttrDists (UnitRace race, UnitClass cls, float typeWeight = 0.5f)
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

		public Unidad MakeEnemy (LogicGrid grid, float exp = 0)
		{
			var ret = new Unidad (grid, TextureString);
			var uClass = PossibleClasses [_r.Next (PossibleClasses.Length)];


			foreach (var x in mergeAttrDists (this, uClass))
				ret.Exp.AddAssignation (x.Key, x.Value);
			ret.Exp.ExperienciaAcumulada = exp;
			ret.Exp.Flush ();

			ret.RecursoHP.Fill ();
			ret.Inteligencia = new Inteligencia.ChaseIntelligence (ret);
			ret.Nombre = Name;

			// TODO: Add drops

			DebugAllInfo (ret);
			return ret;

		}

		[JsonConstructor]
		public UnitRace (string Name,
		                 UnitClass [] PossibleClasses,
		                 ReadOnlyDictionary<string, float> AttributesDistribution,
		                 string TextureString)
		{
			this.Name = Name;
			this.PossibleClasses = PossibleClasses;
			this.AttributesDistribution = AttributesDistribution;
			this.TextureString = TextureString;
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