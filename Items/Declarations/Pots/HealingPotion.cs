
namespace Items.Declarations.Pots
{
	public class HealingPotion : CommonItemBase
	{
		public HealingPotion ()
			: base ("Healing Potion")
		{
			Color = Microsoft.Xna.Framework.Color.Red;
			TextureName = "potion";
		}
	}
}