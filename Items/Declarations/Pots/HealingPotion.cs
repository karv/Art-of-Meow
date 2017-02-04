using Skills;
using Units;
using Units.Recursos;
using Units.Skills;

namespace Items.Declarations.Pots
{
	/// <summary>
	/// Healing potion.
	/// </summary>
	public class HealingPotion : CommonItemBase, 
	ISkill // Hace usable este item
	{
		/// <summary>
		/// Cantidad de HP que cura
		/// </summary>
		protected virtual float HealHp { get; set; }

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
		void ISkill.GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			LastGeneratedInstance.Effects.AddEffect (
				new RemoveItemEffect (user, user, this));
			LastGeneratedInstance.Effects.AddEffect (
				new ChangeRecurso (user, user, ConstantesRecursos.HP, HealHp){ ShowDeltaLabel = true });


		}

		bool ISkill.IsCastable (IUnidad user)
		{
			// return true, si el objeto no está en el inventario no aparecerá
			return true; 
		}

		bool ISkill.IsVisible (IUnidad user)
		{
			// return true, si el objeto no está en el inventario no aparecerá
			return true;
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public override object Clone ()
		{
			return new HealingPotion
			{
				HealHp = this.HealHp,
				TextureName = this.TextureName,
				Color = this.Color
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Pots.HealingPotion"/> class.
		/// </summary>
		public HealingPotion ()
			: base ("Healing Potion")
		{
			HealHp = 5f;
			Color = Microsoft.Xna.Framework.Color.Red;
			TextureName = "heal";
		}
	}
}