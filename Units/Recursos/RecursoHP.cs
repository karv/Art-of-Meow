using System;
using AoM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Units.Recursos
{
	/// <summary>
	/// Maneja el recurso especial HP
	/// </summary>
	public class RecursoHP : Recurso, IVisibleRecurso
	{
		/// <summary>
		/// The color of the background
		/// </summary>
		public readonly Color BgColor = Color.Black * 0.4f;
		/// <summary>
		/// The filling color
		/// </summary>
		public readonly Color FillColor = Color.Red;

		/// <summary>
		/// Dibuja el objeto sobre un rectángulo específico
		/// </summary>
		/// <param name="batch">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch batch, Rectangle rect)
		{
			var text = Juego.Textures.SolidTexture;
			batch.Draw (text, rect, BgColor);

			/* Horizontal*/
			var fullRect = new Rectangle (
				               rect.Location,
				               new Point (
					               (int)(RelativeHp * rect.Width),
					               rect.Height));

			batch.Draw (text, fullRect, FillColor);
		}

		/// <summary>
		/// Gets the short name.
		/// </summary>
		/// <returns>The short name.</returns>
		protected override string GetShortName ()
		{
			return NombreCorto;
		}

		/// <summary>
		/// Gets the long name.
		/// </summary>
		/// <returns>The long name.</returns>
		protected override string GetLongName ()
		{
			return NombreLargo;
		}

		/// <summary>
		/// Gets the name of the unique.
		/// </summary>
		/// <returns>The unique name.</returns>
		protected override string GetUniqueName ()
		{
			return NombreÚnico;
		}

		/// <summary>
		/// Nombre (debe ser único en el manejador de recursos) del recurso
		/// </summary>
		public string NombreÚnico { get { return "hp"; } }

		/// <summary>
		/// Nombre corto
		/// </summary>
		public string NombreCorto { get { return "HP"; } }

		/// <summary>
		/// Nombre largo
		/// </summary>
		public string NombreLargo { get { return "Hit points"; } }

		float _valor;

		/// <summary>
		/// Gets or sets the current value of HP
		/// </summary>
		/// <value>The valor.</value>
		public override float Valor
		{
			get { return _valor; }
			set
			{
				var before = _valor;
				_valor = Math.Min (Math.Max (value, 0), Max);

				// Si no hay cambio, regresar inmediatamente
				if (_valor == before)
					return;
				ValorChanged?.Invoke (this, before);

				if (_valor == Max)
					ReachedMax?.Invoke (this, EventArgs.Empty);
				if (_valor == 0)
					ReachedZero?.Invoke (this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Gets or sets the max HP value 
		/// </summary>
		public float Max { get; set; }

		/// <summary>
		/// Gets the relative value.  Value / MaxValue
		/// </summary>
		public float RelativeHp { get { return _valor / Max; } }

		/// <summary>
		/// Sets the HP to the max
		/// </summary>
		public void Fill ()
		{
			Valor = Max;
		}

		/// <summary>
		/// Sets the HP to zero
		/// </summary>
		public void Empty ()
		{
			Valor = 0;
		}

		float IVisibleRecurso.PctValue (float value)
		{
			return value / Max;
		}

		bool IVisibleRecurso.Visible { get { return true; } }

		string IVisibleRecurso.TextureFill { get { return "pixel"; } }

		Color IVisibleRecurso.FullColor { get { return Color.Red; } }

		/// <summary>
		/// Ocurre cuando su valor cambia,
		/// su argumento dice su valor antes del cambio
		/// </summary>
		public event EventHandler<float> ValorChanged;
		/// <summary>
		/// Occurs when reached zero.
		/// </summary>
		public event EventHandler ReachedZero;
		/// <summary>
		/// Occurs when reached max.
		/// </summary>
		public event EventHandler ReachedMax;

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Units.Recursos.RecursoHP"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Units.Recursos.RecursoHP"/>.</returns>
		public override string ToString ()
		{
			return base.ToString () + string.Format (
				"{0}: {1}/{2}",
				NombreCorto,
				Valor,
				Max);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Units.Recursos.RecursoHP"/> class.
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		public RecursoHP (IUnidad unidad)
			: base (unidad)
		{
		}
	}
}