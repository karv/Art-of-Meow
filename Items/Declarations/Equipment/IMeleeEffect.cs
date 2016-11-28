using Units;
using Skills;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Manager the melee interaction between user and an adjacent foe
	/// </summary>
	public interface IMeleeEffect
	{
		/// <summary>
		/// Devuelve el efecto que causa este ataque melee sobre un target
		/// </summary>
		/// <param name="user">Agente</param>
		/// <param name="target">Target</param>
		IEffect GetEffect (IUnidad user, IUnidad target);
	}
}