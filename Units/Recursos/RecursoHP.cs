using System;
using Componentes;
using AoM;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Units.Recursos
{
	public class RecursoHP : IRecurso, IRelDraw
	{
		public readonly Color BgColor = Color.DarkBlue * 0.2f;
		public readonly Color FillColor = Color.Red;

		public void Update (float gameTime)
		{
		}

		public void Draw (Rectangle topLeft,
		                  SpriteBatch batch)
		{
			var text = Juego.Textures.SolidTexture;
			batch.Draw (text, topLeft, BgColor);
			var fullRect = new Rectangle (
				               topLeft.X,
				               topLeft.Y + (int)(RelativeHp * topLeft.Height),
				               topLeft.Width,
				               topLeft.Height - (int)(topLeft.Y + RelativeHp * topLeft.Height));
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