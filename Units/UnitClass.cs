using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Units.Inteligencia;
using System;

namespace Units
{
	/// <summary>
	/// Represents a unit's job
	/// </summary>
	public class UnitClass
	{
		/// <summary>
		/// Name of the job
		/// </summary>
		public readonly string Name;

		public readonly IUnidadController Int;

		/// <summary>
		/// Attributes of this job
		/// </summary>
		public readonly ReadOnlyDictionary<string, float> AttributesDistribution;

		/// <summary>
		/// Assignment from item name to drop weight.
		/// </summary>
		[JsonIgnore]
		public readonly Items.DropAssignment DropDistribution;

		public static AI GetAIByName (string name)
		{
			switch (name)
			{
				case "Chase":
					return new ChaseIntelligence ();
				case "Ranged":
					return new RangedIntelligence ();
				default:
					throw new Exception ();
			}

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnitClass"/> class.
		/// </summary>
		[JsonConstructor]
		public UnitClass (string Name,
		                  Dictionary<string, float> AttributesDistribution,
		                  Dictionary<string, float> DropDistribution,
		                  string AI)
		{
			if (AI == null)
				throw new System.ArgumentNullException ("AI");
			if (Name == null)
				throw new System.ArgumentNullException ("Name");
			if (AttributesDistribution == null)
				throw new System.ArgumentNullException ("AttributesDistribution");
			if (DropDistribution == null)
				throw new System.ArgumentNullException ("DropDistribution");

			this.Name = Name;
			Int = GetAIByName (AI);
			this.AttributesDistribution = new ReadOnlyDictionary<string, float> (AttributesDistribution);
			this.DropDistribution = new Items.DropAssignment (DropDistribution);
		}
	}
}