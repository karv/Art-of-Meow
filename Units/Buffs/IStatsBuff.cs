using Units.Buffs;

namespace Units.Buffs
{
	/// <summary>
	/// Representa un <see cref="IBuff"/> que afecta los stats
	/// </summary>
	public interface IStatsBuff : IBuff
	{
		/// <summary>
		/// Devuelve la modificación (absoluta) de valor de un recurso.
		/// </summary>
		float StatDelta (string resName);
	}
}