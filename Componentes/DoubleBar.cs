using System;
using Art_of_Meow;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units.Recursos;
using Units;

namespace Componentes
{
	[Obsolete]
	public class StatDoubleBar : DoubleBar
	{
		public IRecurso Recurso { get; }

		public IUnidad Unidad { get; }

		public string NombreRecurso { get; }

		public override float CurrValue
		{
			get
			{
				return Recurso.Valor;
			}
			set
			{
				Recurso.Valor = value;
			}
		}

		public override float MaxValue
		{
			get
			{
				return 10;
			}
			set
			{
				throw new Exception ();
			}
		}

		public StatDoubleBar (IScreen scr, IUnidad unidad, string nomRec)
			: base (scr)
		{
			Unidad = unidad;
			NombreRecurso = nomRec;

			Recurso = unidad.Recursos.GetRecurso (NombreRecurso);
		}
	}

	[Obsolete]
	public class DoubleBar : SBC
	{
		public virtual float MaxValue { get; set; }

		public virtual float CurrValue { get; set; }

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