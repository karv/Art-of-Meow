
namespace Items.Declarations.Pots
{
	// TODO: incompleto
	/// <summary>
	/// Healing potion.
	/// </summary>
	public class HealingPotion : CommonItemBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Pots.HealingPotion"/> class.
		/// </summary>
		public HealingPotion ()
			: base ("Healing Potion")
		{
			Color = Microsoft.Xna.Framework.Color.Red;
			TextureName = "potion";
		}
	}
}