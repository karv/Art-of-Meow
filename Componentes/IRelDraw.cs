using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Componentes
{
	public interface IRelDraw
	{
		void Draw (Rectangle topLeft, SpriteBatch batch);
	}
}