using System;
using Moggle.Controles;
using Moggle.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Shapes;
using Moggle;

namespace Componentes
{
	// TODO:; No almacenar Bounds como campo, recalcularlo cada vez que se requiera
	// TODO: rename VanishingLabel
	// TEST
	/// <summary>
	/// Vanishing string.
	/// </summary>
	public class VanishingString : DSBC
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Componentes.VanishingString"/> class.
		/// </summary>
		/// <param name="screen">Screen.</param>
		/// <param name="texto">Texto.</param>
		/// <param name="duración">Duración.</param>
		public VanishingString (IScreen screen, string texto, TimeSpan duración)
			: base (screen)
		{
			_texto = texto;
			Restante = duración;
			TiempoInicial = duración;
			ColorFinal = Color.Transparent;
			Velocidad = new Vector2 (0, -20);
		}

		BitmapFont Font;
		string _texto;
		Vector2 topLeft;

		/// <summary>
		/// Velocidad de este control.
		/// </summary>
		public Vector2 Velocidad { get; set; }

		/// <summary>
		/// Devuelve el tiempo restante para desaparecer y liberarse
		/// </summary>
		/// <value>The restante.</value>
		public TimeSpan Restante { get; private set; }

		/// <summary>
		/// Gets the total duration of the label
		/// </summary>
		/// <value>The tiempo inicial.</value>
		public TimeSpan TiempoInicial { get; }

		/// <summary>
		/// Gets or sets the text of the label
		/// </summary>
		/// <value>The texto.</value>
		public string Texto
		{
			get
			{
				return _texto;
			}
			set
			{
				_texto = value;
				calcularBounds ();
			}
		}

		/// <summary>
		/// Recalculates the value of <see cref="Bounds"/> field
		/// </summary>
		void calcularBounds ()
		{
			Bounds = Font.GetStringRectangle (Texto, TopLeft);
		}

		/// <summary>
		/// Gets or sets the top left corner's coordinates
		/// </summary>
		/// <value>The top left.</value>
		public Vector2 TopLeft
		{
			get
			{
				return topLeft;
			}
			set
			{
				topLeft = value;
				calcularBounds ();
			}
		}

		/// <summary>
		/// Gets or sets the center of the label
		/// </summary>
		/// <value>The centro.</value>
		public Vector2 Centro
		{
			get
			{
				return Bounds.Center;
			}
			set
			{
				var altura = Bounds.Height;
				var grosor = Bounds.Width;
				topLeft = value - new Vector2 (grosor / 2.0f, altura / 2.0f);
				calcularBounds ();
			}
		}

		/// <summary>
		/// Gets the bounds of the label
		/// </summary>
		public RectangleF Bounds { get; private set; }

		/// <summary>
		/// Gets the bounds of the label
		/// </summary>
		protected override IShapeF GetBounds ()
		{
			return Bounds;
		}

		/// <summary>
		/// Gets or sets the starting color
		/// </summary>
		public Color ColorInicial { get; set; }

		/// <summary>
		/// Gets or sets the end color
		/// </summary>
		/// <value>The color final.</value>
		public Color ColorFinal { get; set; }

		/// <summary>
		/// Gets the current color
		/// </summary>
		public Color ColorActual
		{
			get
			{
				var t = escalarColor;
				var ret = new Color (
					          (int)(ColorInicial.R * t + ColorFinal.R * (1 - t)),
					          (int)(ColorInicial.G * t + ColorFinal.G * (1 - t)),
					          (int)(ColorInicial.B * t + ColorFinal.B * (1 - t)),
					          (int)(ColorInicial.A * t + ColorFinal.A * (1 - t))
				          );
				return ret;
			}
		}



		/// <summary>
		/// Devuelve valor en [0, 1] depende de dónde en el tiempo es el estado actual de este control (lineal)
		/// 0 si está en el punto de terminación
		/// 1 si está en el punto de inicio
		/// </summary>
		float escalarColor
		{
			get
			{
				return (float)Restante.Ticks / TiempoInicial.Ticks;
			}
		}

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		protected override void Draw (GameTime gameTime)
		{
			if (Font != null)
				Screen.Batch.DrawString (Font, Texto, TopLeft, ColorActual);
		}

		/// <summary>
		/// Loads the font
		/// </summary>
		/// <param name="manager">Manager.</param>
		protected override void AddContent (BibliotecaContenido manager)
		{
			manager.AddContent ("fonts");
		}

		/// <summary>
		/// Carga el valor de <see cref="Font"/>
		/// </summary>
		protected override void InitializeContent (BibliotecaContenido manager)
		{
			Font = Screen.Content.GetContent<BitmapFont> ("fonts");
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Componentes.VanishingString"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Componentes.VanishingString"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Componentes.VanishingString"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Componentes.VanishingString"/> so
		/// the garbage collector can reclaim the memory that the <see cref="Componentes.VanishingString"/> was occupying.</remarks>
		protected override void Dispose ()
		{
			Font = null;
			base.Dispose ();
		}

		/// <summary>
		/// Update lógico
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public override void Update (GameTime gameTime)
		{
			Restante -= gameTime.ElapsedGameTime;
			if (Restante < TimeSpan.Zero)
				OnTerminar ();
			else
				TopLeft += Velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		/// <summary>
		/// terminates the control
		/// </summary>
		protected void OnTerminar ()
		{
			Finished?.Invoke (this, EventArgs.Empty);
			Finished = null;
			Screen.RemoveComponent (this);
			Dispose ();
		}

		/// <summary>
		/// Se ejecuta antes del ciclo, pero después de saber un poco sobre los controladores.
		/// No invoca LoadContent por lo que es seguro agregar componentes
		/// </summary>
		public override void Initialize ()
		{
			base.Initialize ();
			calcularBounds ();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Componentes.VanishingString"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Componentes.VanishingString"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[VanishingString: _texto={0}, Restante={1}, TiempoInicial={2}]",
				_texto,
				Restante,
				TiempoInicial);
		}

		/// <summary>
		/// Ocurrs when the label is about to be disposed
		/// </summary>
		public event EventHandler Finished;
	}
}