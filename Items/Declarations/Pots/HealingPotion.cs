
namespace Items.Declarations.Pots
{
	public class HealingPotion : CommonItemBase
	{
		protected override Microsoft.Xna.Framework.Color GetColor ()
		{
			return Microsoft.Xna.Framework.Color.Red;
		}

		public override string Nombre
		{
			get
			{
				return "Healing Potion";
			}
		}

		public override string TextureName
		{
			get
			{
				// TODO: este contenido no existe aún.
				return "potion";
			}
		}

		public HealingPotion ()
			: base ()
		{
		}
	}
}