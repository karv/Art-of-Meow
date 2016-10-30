using Units;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Manager the melee interaction between user and an adjacent foe
	/// </summary>
	public interface IMeleeEffect
	{
		void DoMeleeEffectOn (IUnidad user, IUnidad target);
	}
}