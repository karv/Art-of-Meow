using System;
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
		/// The base cooldown for the user
		/// </summary>
		public float CoolDownBase { get; }

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="user">User.</param>
		protected abstract void GetInstance (IUnidad user);

		/// <summary>
		/// Determines whether this instance is castable by the specified user.
		/// ie. Is Enabled?
		/// </summary>
		protected abstract bool IsCastable (IUnidad user);

		/// <summary>
		/// Determines whether this instance is visible for the specified user.
		/// </summary>
		protected abstract bool IsVisible (IUnidad user);

		/// <summary>
		/// Adds the cooldown effect to a <see cref="CollectionEffect"/>
		/// </summary>
		protected void AddCooldownEffect (IUnidad user, CollectionEffect effects)
		{
			if (CoolDownBase == 0)
				return;
			if (CoolDownBase < 0)
				throw new Exception ();
			// TODO: check for user's speed or something
			effects.AddEffect (new GenerateCooldownEffect (user, user, CoolDownBase), true);
		}

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

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.UsableItem"/> class.
		/// </summary>
		protected UsableItem (string name, float cooldown)
			: base (name)
		{
			CoolDownBase = cooldown;
		}
	}
}