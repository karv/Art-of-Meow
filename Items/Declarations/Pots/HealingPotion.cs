using System;
using Units;
using Units.Order;
using Units.Recursos;
using Units.Skills;
using Skills;

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

		public SkillInstance LastGeneratedInstance { get; protected set; }

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="user">User.</param>
		void ISkill.GetInstance (IUnidad user)
		{
			LastGeneratedInstance = new SkillInstance (this, user);
			LastGeneratedInstance.AddEffect (new RemoveItemEffect (user, user, this));
			LastGeneratedInstance.AddEffect (
				new ChangeRecurso (user, user, ConstantesRecursos.HP, HealHp));

		}

		void doEffect (IUnidad target)
		{
			//var hpRec = target.Recursos.GetRecurso (ConstantesRecursos.HP);
			//hpRec.Valor += HealHp;
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
		/// Ocurre cuando se termina la ejecución
		/// </summary>
		public event EventHandler Executed;

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