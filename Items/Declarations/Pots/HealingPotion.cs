using Units.Skills;
using Units;
using Units.Order;
using Units.Recursos;

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
		/// Executes this skill
		/// </summary>
		/// <param name="user">The caster</param>
		void ISkill.Execute (IUnidad user)
		{
			user.EnqueueOrder (new ExecuteOrder (user, doEffect));
		}

		void doEffect (IUnidad target)
		{
			var hpRec = target.Recursos.GetRecurso (ConstantesRecursos.HP);
			hpRec.Valor += HealHp;
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