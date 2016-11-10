using Units;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Manager the melee interaction between user and an adjacent foe
	/// </summary>
	public interface IMeleeEffect
	{
		/// <summary>
		/// Do the melee effect from a Unidad to its target
		/// </summary>
		/// <param name="user">User of the melee move</param>
		/// <param name="target">Target.</param>
		void DoMeleeEffectOn (IUnidad user, IUnidad target);
	}
}