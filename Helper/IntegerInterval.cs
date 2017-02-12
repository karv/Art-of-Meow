using System;
using Newtonsoft.Json;

namespace Helper
{
	/// <summary>
	/// Represents a distibution of equiprobable integers which are bounded for fixed limits
	/// </summary>
	public struct IntegerInterval : IDistribution<int>
	{
		readonly Random _r;

		/// <summary>
		/// Gets the minimum value
		/// </summary>
		/// <value>The minimum.</value>
		public int Min { get; }

		/// <summary>
		/// Gets the maximum value
		/// </summary>
		/// <value>The max.</value>
		public int Max { get; }

		/// <summary>
		/// Picks a number in the interval (inclusive)
		/// </summary>
		public int Pick ()
		{
			return _r.Next (Min, Max);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Helper.IntegerInterval"/> struct.
		/// </summary>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum</param>
		[JsonConstructor]
		public IntegerInterval (int min, int max)
		{
			if (min > max)
				throw new Exception ();

			Min = min;
			Max = max;
			_r = new Random ();
		}
	}
}