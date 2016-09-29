using Moggle.Controles;

namespace Units.Skills
{
	public interface ISkill : IDibujable, IComponent
	{
		void Execute (IUnidad user);

		bool IsCastable (IUnidad user);

		bool IsVisible (IUnidad user);
	}
}