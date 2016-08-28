using Moggle.Controles;
using Moggle.Screens;
using Microsoft.Xna.Framework;
using Art_of_Meow;

namespace Componentes
{
	public class DoubleBar : SBC
	{
		public float MaxValue { get; set; }

		public float CurrValue { get; set; }

		public float RelativeValue
		{
			get { return CurrValue / MaxValue; }
		}

		public Moggle.Shape.Rectangle Bounds { get; set; }

		public Color BgColor { get; set; }

		public Color FgColor { get; set; }

		public override void Dibujar (GameTime gameTime)
		{
			var rec = (Rectangle)Bounds;
			var bat = Screen.Batch;
			bat.Draw (Juego.Textures.SolidTexture, rec, BgColor);
			var fgRect = new Rectangle (
				             rec.Location, 
				             new Point (
					             (int)(rec.Width * RelativeValue),
					             (int)Bounds.Height));

			bat.Draw (Juego.Textures.SolidTexture, fgRect, FgColor);
		}

		public override void LoadContent ()
		{
		}

		public override Moggle.Shape.IShape GetBounds ()
		{
			return Bounds;
		}

		public DoubleBar (IScreen scr)
			: base (scr)
		{
			BgColor = Color.Transparent;
			FgColor = Color.White;
		}
	}
}