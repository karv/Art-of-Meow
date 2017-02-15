using Skills;
using Units;
using Units.Skills;

namespace Items.Declarations
{
	/// <summary>
	/// Healing potion.
	/// </summary>
	public abstract class UsableItem : CommonItemBase, 
	ISkill // Hace usable este item
	{

		/// <summary>
		/// Devuelve la última instancia generada.
		/// </summary>
		/// <value>The last generated instance.</value>
		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="user">User.</param>
		protected abstract void GetInstance (IUnidad user);

		protected abstract bool IsCastable (IUnidad user);

		protected abstract bool IsVisible (IUnidad user);

		void ISkill.GetInstance (IUnidad user)
		{
			GetInstance (user);
		}

		bool ISkill.IsCastable (IUnidad user)
		{
			return IsCastable (user);
		}

		bool ISkill.IsVisible (IUnidad user)
		{
			return IsVisible (user);
		}

		protected UsableItem (string name)
			: base (name)
		{
			
		}
	}
}