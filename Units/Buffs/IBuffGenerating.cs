using System.Collections.Generic;
using Units.Buffs;

namespace Units.Buffs
{
	/// <summary>
	/// Es un objeto que puede ir acumulando y generando un buff.
	/// ie Equipment que cambia estado
	/// </summary>
	public interface IBuffGenerating
	{
		/// <summary>
		/// Enumera los stats y la cantidad que son modificados.
		/// </summary>
		IEnumerable<KeyValuePair<string, float>> GetDeltaStat ();
	}
}