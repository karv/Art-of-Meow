using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Componentes
{
	public interface IRelDraw
	{
		/// <summary>
		/// Draws this object in a position (screen-wise)
		/// </summary>
		/// <param name="area">TopLeft of the output.</param>
		/// <param name="batch">SpriteBatch de dibujo</param>
		void Draw (Rectangle area, SpriteBatch batch);
	}
}