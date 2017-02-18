using System;
using AoM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using System.Diagnostics;
using Debugging;

namespace Units.Recursos
{
	/// <summary>
	/// Maneja el recurso especial HP
	/// </summary>
	public class RecursoHP : Recurso, IVisibleRecurso
	{
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

			/* Horizontal*/
			var fullRect = new Rectangle (
				               rect.Location,
				               new Point (
					               (int)(RelativeHp * rect.Width),
					               rect.Height));

			batch.Draw (text, fullRect, FillColor);
		}

		void IComponent.LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
		}

		void IGameComponent.Initialize ()
		{
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
		/// Gets the unique name
		/// </summary>
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

		float _currValue;

		/// <summary>
		/// Gets or sets the current value of HP
		/// </summary>
		/// <value>The valor.</value>
		public override float Valor
		{
			get { return _currValue; }
			set
			{ 
				value = Math.Max (Math.Min (value, Max), 0);
				if (_currValue == value)
					return;
				var prevVal = _currValue;
				_currValue = value; 

				ValorChanged?.Invoke (this, prevVal);
				if (value == 0)
					ReachedZero?.Invoke (this, EventArgs.Empty);
				else if (value == Max)
					ReachedMax?.Invoke (this, EventArgs.Empty);
			}
		}

		void _re_bound_value ()
		{
			Valor = Valor;
		}

		/// <summary>
		/// Gets or sets the max HP value 
		/// </summary>
		public float Max
		{
			get { return _Max.ModifiedValue; }
			set
			{ 
				_Max.Valor = value; 
				_re_bound_value ();// This updates the value to its new bound.
			}
		}

		/// <summary>
		/// Gets the relative value.  Value / MaxValue
		/// </summary>
		public float RelativeHp { get { return Valor / Max; } }

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

		RegenParam Regen { get; }

		MaxHpParam _Max { get; }

		bool IVisibleRecurso.Visible { get { return true; } }

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
			return string.Format (
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
			Regen = new RegenParam (this);
			_Max = new MaxHpParam (this);
			Parámetros.Add (Regen);
			Parámetros.Add (_Max);
		}

		class MaxHpParam : IParámetroRecurso
		{
			public void ReceiveExperience (float exp)
			{
				Valor += 100 * exp;
				Debug.WriteLine ("MaxHp += " + 100 * exp, DebugCategories.Experience);
			}

			const float expGainThreshold = 0.25f;
			const float expGameRate = 0.9f;

			IRecurso _recurso { get; }

			IRecurso IParámetroRecurso.Recurso{ get { return _recurso; } }

			public IUnidad Unidad { get { return Recurso.Unidad; } }

			public float ModifiedValue
			{ get { return Valor + Unidad.Recursos.RecursoExtra (ConstantesRecursos.HP + ".max"); } }

			public float Valor { get; set; }

			public void Update (float gameTime)
			{
				// pedir exp
				if (Recurso.RelativeHp < expGainThreshold)
					_recurso.Unidad.Exp.AddAssignation (this, gameTime * expGameRate);
			}

			public RecursoHP Recurso { get { return _recurso as RecursoHP; } }

			public string NombreÚnico { get { return "max"; } }

			public MaxHpParam (RecursoHP recurso)
			{
				_recurso = recurso;
			}
		}

		class RegenParam : IParámetroRecurso
		{
			public void ReceiveExperience (float exp)
			{
				Valor += exp;
			}

			const float expGainThreshold = 0.5f;

			const float expGameRate = 0.01f;

			public void Update (float gameTime)
			{
				Recurso.Valor += Valor * gameTime * 20;

				// pedir exp
				if (Recurso.RelativeHp < expGainThreshold)
					Recurso.Unidad.Exp.AddAssignation (this, gameTime * expGameRate);
			}

			public RecursoHP Recurso { get; }

			IRecurso IParámetroRecurso.Recurso { get { return Recurso; } }

			public string NombreÚnico { get { return "regen"; } }

			public float Valor { get; set; }

			public RegenParam (RecursoHP recBase)
			{
				Recurso = recBase;
			}
		}
	}
}