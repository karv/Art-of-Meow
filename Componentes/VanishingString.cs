using System;
using Moggle.Controles;
using Moggle.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Shapes;
using Microsoft.Xna.Framework.Content;

namespace Componentes
{
	public class VanishingString : DSBC
	{
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

		string _fontName;

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		public string FontName
		{
			get
			{
				return _fontName;
			}
			set
			{
				if (Font != null)
					throw new InvalidOperationException ("Cannot change font name after content is loaded.");
				_fontName = value;
			}
		}

		/// <summary>
		/// Velocidad de este control.
		/// </summary>
		public Vector2 Velocidad { get; set; }

		public TimeSpan Restante { get; private set; }

		public TimeSpan TiempoInicial { get; }

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

		void calcularBounds ()
		{
			Bounds = Font.GetStringRectangle (Texto, TopLeft);
		}

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

		public RectangleF Bounds { get; private set; }

		public override IShapeF GetBounds ()
		{
			return Bounds;
		}

		public Color ColorInicial { get; set; }

		public Color ColorFinal { get; set; }

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

		public override void Draw (GameTime gameTime)
		{
			if (Font != null)
				Screen.Batch.DrawString (Font, Texto, TopLeft, ColorActual);
		}

		protected override void LoadContent (ContentManager manager)
		{
			Font = Screen.Content.Load<BitmapFont> (FontName);
		}

		protected override void Dispose ()
		{
			Font = null;
			base.Dispose ();
		}

		public override void Update (GameTime gameTime)
		{
			Restante -= gameTime.ElapsedGameTime;
			if (Restante < TimeSpan.Zero)
				OnTerminar ();
			else
				TopLeft += Velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		protected void OnTerminar ()
		{
			Dispose ();
			AlTerminar?.Invoke (this, EventArgs.Empty);
			AlTerminar = null;
		}

		public override void Initialize ()
		{
			base.Initialize ();
			calcularBounds ();
		}

		public event EventHandler AlTerminar;
	}
}