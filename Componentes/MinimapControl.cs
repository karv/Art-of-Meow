using Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended;

namespace Componentes
{
	public class MinimapControl : SBC
	{
		public MemoryGrid DisplayingGrid { get; set; }

		public Texture2D GenerateTexture ()
		{
			var size = DisplayingGrid.MemorizingGrid.Size;
			var ret = new Texture2D (Game.GraphicsDevice, size.Width, size.Height);

			var _data = new Color[size.Width * size.Height];
			for (int i = 0; i < size.Width * size.Height; i++)
			{
				var px = i / size.Width;
				var py = i % size.Width;
				_data [i] = DisplayingGrid [new Point (px, py)].MinimapColor;
			}

			ret.SetData<Color> (_data);
			return ret;
		}

		public MinimapControl (IComponentContainerComponent<IControl> cont)
			: base (cont)
		{
		}
		
	}
}