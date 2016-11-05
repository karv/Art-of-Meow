using Units.Skills;
using Units;
using Units.Order;
using Units.Recursos;


namespace Items.Declarations.Pots
{
	// TODO: incompleto
	/// <summary>
	/// Healing potion.
	/// </summary>
	public class HealingPotion : CommonItemBase, 
	ISkill // Hace usable este item
	{
		protected float HealHp = 5f;

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

		public bool IsCastable (IUnidad user)
		{
			// return true, si el objeto no está en el inventario no aparecerá
			return true; 
		}

		public bool IsVisible (IUnidad user)
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
			Color = Microsoft.Xna.Framework.Color.Red;
			TextureName = "heal";
		}
	}
}