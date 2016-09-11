using System;
using Componentes;
using AoM;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	public class RecursoHP : IRecurso, IRelDraw
	{
		public readonly Color BgColor = Color.Black * 0.4f;
		public readonly Color FillColor = Color.Red;

		public void Update (float gameTime)
		{
		}

		public void Draw (Rectangle rect,
		                  SpriteBatch batch)
		{
			var text = Juego.Textures.SolidTexture;
			batch.Draw (text, rect, BgColor);
			/* Vertical
			var fullRect = new Rectangle (
				               topLeft.X,
				               topLeft.Bottom - (int)(RelativeHp * topLeft.Height),
				               topLeft.Width,
				               (int)(RelativeHp * topLeft.Height));
			*/

			/* Horizontal*/
			var fullRect = new Rectangle (
				               rect.Location,
				               new Point (
					               (int)(RelativeHp * rect.Width),
					               rect.Height));

			batch.Draw (text, fullRect, FillColor);
		}

		public string NombreÚnico { get { return "hp"; } }

		public string NombreCorto { get { return "HP"; } }

		public string NombreLargo { get { return "Hit points"; } }

		float _valor;

		public float Valor
		{
			get { return _valor; }
			set
			{
				_valor = Math.Min (Math.Max (value, 0), Max);
			}
		}

		public float Max { get; set; }

		public float RelativeHp { get { return _valor / Max; } }

		public void Fill ()
		{
			_valor = Max;
		}

		public void Empty ()
		{
			_valor = 0;
		}

		public IUnidad Unidad { get; }

		public override string ToString ()
		{
			return string.Format ("{0}: {1}/{2}", NombreCorto, Valor, Max);
		}

		public RecursoHP (IUnidad unidad)
		{
			Unidad = unidad;
		}
	}
}