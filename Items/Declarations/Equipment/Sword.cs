
namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : MeleeWeapon
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.Equipment.Sword"/> class.
		/// </summary>
		public Sword ()
			: base ("Sword", @"Items/katana", 0.9f, 0.7f)
		{
			Color = Microsoft.Xna.Framework.Color.Black;
		}
	}
}