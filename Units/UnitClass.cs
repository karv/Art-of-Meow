using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Units.Inteligencia;

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

		public string [] StartingEquipment = new string[0];

		/// <summary>
		/// Assignment from item name to drop weight.
		/// </summary>
		[JsonIgnore]
		public readonly Items.DropAssignment DropDistribution;

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnitClass"/> class.
		/// </summary>
		[JsonConstructor]
		public UnitClass (string Name,
		                  Dictionary<string, float> AttributesDistribution,
		                  Dictionary<string, float> DropDistribution,
		                  string AI,
		                  string [] StartingEquipment)
		{
			if (AI == null)
				throw new ArgumentNullException ("AI");
			if (Name == null)
				throw new ArgumentNullException ("Name");
			if (AttributesDistribution == null)
				throw new ArgumentNullException ("AttributesDistribution");
			if (DropDistribution == null)
				throw new ArgumentNullException ("DropDistribution");

			this.StartingEquipment = StartingEquipment;
			this.Name = Name;
			Int = Units.Inteligencia.AI.GetAIByName (AI);
			this.AttributesDistribution = new ReadOnlyDictionary<string, float> (AttributesDistribution);
			this.DropDistribution = new Items.DropAssignment (DropDistribution);
		}
	}
}