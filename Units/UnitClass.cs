using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Cells;
using Debugging;
using Newtonsoft.Json;
using AoM;

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
		/// <summary>
		/// Attributes of this job
		/// </summary>
		public readonly ReadOnlyDictionary<string, float> AttributesDistribution;

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.UnitClass"/> class.
		/// </summary>
		[JsonConstructor]
		public UnitClass (string Name, Dictionary<string, float> AttributesDistribution)
		{
			this.Name = Name;
			this.AttributesDistribution = new ReadOnlyDictionary<string, float> (AttributesDistribution);
		}
		
	}
}