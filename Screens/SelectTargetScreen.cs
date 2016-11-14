using Cells;
using Componentes;
using Moggle.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Net.Mime;
using System;
using MonoGame.Extended.InputListeners;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using AoM;
using MonoGame.Extended;

namespace Screens
{
	public class SelectableGridControl : GridControl
	{
		Point cursorPosition;

		public Point CursorPosition
		{
			get
			{
				return cursorPosition;
			}
			set
			{
				// No permitir un valor fuera del universo de Grid
				cursorPosition = new Point (
					Math.Min (Math.Max (value.X, 0), Grid.Size.Width),
					Math.Min (Math.Max (value.Y, 0), Grid.Size.Height));
				OnCursorMoved ();
			}
		}

		public Color CursorColor { get; set; }

		Texture2D pixel;

		protected override void Draw (GameTime gameTime)
		{
			base.Draw (gameTime);

			// Dibujar el seleccionado
			var bat = Screen.Batch;
			var loc = CellSpotLocation (CursorPosition);
			var rectOut = new Rectangle (loc, (Size)CellSize);
			bat.Draw (
				pixel,
				rectOut,
				CursorColor);
		}

		protected override void InitializeContent ()
		{
			base.InitializeContent ();
			pixel = Screen.Content.GetContent<Texture2D> ("pixel");
		}

		public event EventHandler CursorMoved;

		protected virtual void OnCursorMoved (EventArgs e)
		{
			if (!IsVisible (CursorPosition))
				TryCenterOn (CursorPosition);
			CursorMoved?.Invoke (this, e);
		}

		protected void OnCursorMoved ()
		{
			OnCursorMoved (EventArgs.Empty);
		}

		public SelectableGridControl (LogicGrid grid, IScreen scr)
			: base (grid, scr)
		{
			CursorColor = Color.LightGreen * 0.3f;
		}
	}

	public class SelectTargetScreen : DialScreen
	{
		// No dibujar base, hay que dibujar otro tipo-GridControl más apropiado
		public override bool DibujarBase { get { return false; } }

		public LogicGrid Grid { get; }

		public SelectableGridControl GridSelector { get; }

		public override void Initialize ()
		{
			base.Initialize ();

			// Poner a GridSelector donde debe
			// TODO?
		}

		public Keys UpKey = Keys.Up;
		public Keys DownKey = Keys.Down;
		public Keys LeftKey = Keys.Left;
		public Keys RightKey = Keys.Right;

		public override void Draw (GameTime gameTime)
		{
			base.Draw (gameTime);
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}

		public override bool RecibirSeñal (KeyboardEventArgs key)
		{
			if (key.Key == UpKey)
			{
				GridSelector.CursorPosition += new Point (0, -1);
				return true;
			}
			if (key.Key == DownKey)
			{
				GridSelector.CursorPosition += new Point (0, 1);
				return true;
			}
			if (key.Key == LeftKey)
			{
				GridSelector.CursorPosition += new Point (-1, 0);
				return true;
			}
			if (key.Key == RightKey)
			{
				GridSelector.CursorPosition += new Point (1, 0);
				return true;
			}
			// No mandar señal al otro diálogo que me invocó
			// return base.RecibirSeñal (key);

			// TODO Necesito poder recuoerar todos los screens
		}

		public SelectTargetScreen (Juego game, LogicGrid grid)
			: base (game)
		{
			Grid = grid;
			GridSelector = new SelectableGridControl (Grid, this);
			AddComponent (GridSelector);
		}
		
	}
}