using System;
using System.Linq;
using AoM;
using Cells;
using Componentes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Moggle.Screens;
using MonoGame.Extended.InputListeners;

namespace Screens
{
	/// <summary>
	/// Un control que hace que se pueda seleccionar un punto en un <see cref="LogicGrid"/>
	/// </summary>
	public class SelectableGridControl : GridControl
	{
		Point cursorPosition;

		/// <summary>
		/// Devuelve o establece la posición del cursor
		/// </summary>
		/// <value>The cursor position.</value>
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

		/// <summary>
		/// Color del cursor
		/// </summary>
		/// <value>The color of the cursor.</value>
		public Color CursorColor { get; set; }

		Texture2D pixel;

		/// <summary>
		/// Dibuja el control.
		/// </summary>
		protected override void Draw ()
		{
			base.Draw ();

			// Dibujar el seleccionado
			var bat = Screen.Batch;
			var loc = CellSpotLocation (CursorPosition);
			var rectOut = new Rectangle (loc, CellSize);
			bat.Draw (
				pixel,
				rectOut,
				CursorColor);
		}

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		protected override void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			base.LoadContent (manager);
			pixel = manager.Load <Texture2D> ("pixel");
		}

		/// <summary>
		/// Ocurre cuando se mueve el cursor
		/// </summary>
		public event EventHandler CursorMoved;

		/// <summary>
		/// Raises the cursor moved event.
		/// </summary>
		/// <param name="e">E.</param>
		protected virtual void OnCursorMoved (EventArgs e)
		{
			if (!IsVisible (CursorPosition))
				TryCenterOn (CursorPosition);
			CursorMoved?.Invoke (this, e);
		}

		/// <summary>
		/// Raises the cursor moved event.
		/// </summary>
		protected void OnCursorMoved ()
		{
			OnCursorMoved (EventArgs.Empty);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.SelectableGridControl"/> class.
		/// </summary>
		public SelectableGridControl (LogicGrid grid, IScreen scr)
			: base (grid, scr)
		{
			CursorColor = Color.LightGreen * 0.3f;
		}
	}

	/// <summary>
	/// Pantalla de selección de target
	/// </summary>
	public class SelectTargetScreen : Screen, Moggle.Screens.Dials.IRespScreen
	{
		/// <summary>
		/// Grid lógico
		/// </summary>
		/// <value>The grid.</value>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Selector de punto en grid
		/// </summary>
		/// <value>The grid selector.</value>
		public SelectableGridControl GridSelector { get; }

		/// <summary>
		/// Ocurre cuando hay selección
		/// </summary>
		public event EventHandler<object> HayRespuesta;

		/// <summary>
		/// Tecla de movimiento de cursor: arriba
		/// </summary>
		public Keys UpKey = Keys.Up;
		/// <summary>
		/// Tecla de movimiento de cursor: abajo
		/// </summary>
		public Keys DownKey = Keys.Down;
		/// <summary>
		/// Tecla de movimiento de cursor: izquierda
		/// </summary>
		public Keys LeftKey = Keys.Left;
		/// <summary>
		/// Tecla de movimiento de cursor: derecha
		/// </summary>
		public Keys RightKey = Keys.Right;
		/// <summary>
		/// Tecla de movimiento de cursor: selección
		/// </summary>
		public Keys SelectKey = Keys.Enter;

		/// <summary>
		/// Rebice señal de un tipo dado
		/// </summary>
		/// <param name="data">Señal recibida</param>
		public override bool RecibirSeñal (Tuple<KeyboardEventArgs, ScreenThread> data)
		{
			var key = data.Item1.Key;
			if (GlobalKeys.SelectUp.Contains (key))
			{
				GridSelector.CursorPosition += new Point (0, -1);
				return true;
			}
			if (GlobalKeys.SelectDownKey.Contains (key))
			{
				GridSelector.CursorPosition += new Point (0, 1);
				return true;
			}
			if (GlobalKeys.SelectLeft.Contains (key))
			{
				GridSelector.CursorPosition += new Point (-1, 0);
				return true;
			}
			if (GlobalKeys.SelectRight.Contains (key))
			{
				GridSelector.CursorPosition += new Point (1, 0);
				return true;
			}
			if (GlobalKeys.Accept.Contains (key))
			{
				HayRespuesta?.Invoke (this, this.GridSelector.CursorPosition);
				data.Item2.TerminateLast ();
				return true;
			}
			if (GlobalKeys.Cancel.Contains (key))
			{
				data.Item2.TerminateLast ();
			}
				
			// No mandar señal al otro diálogo que me invocó
			// return base.RecibirSeñal (key);
			return base.RecibirSeñal (data);
		}

		/// <summary>
		/// </summary>
		/// <param name="game">Game.</param>
		/// <param name="grid">Grid.</param>
		public SelectTargetScreen (Juego game, LogicGrid grid)
			: base (game)
		{
			Grid = grid;
			GridSelector = new SelectableGridControl (Grid, this);
			AddComponent (GridSelector);
		}
		
	}
}