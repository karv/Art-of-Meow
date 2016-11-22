using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Screens;
using System;
using Units;

namespace Helper
{
	/// <summary>
	/// Provee métodos para ayudar en la selección de un punto en un <see cref="LogicGrid"/>
	/// </summary>
	public sealed class SelectorController
	{
		/// <summary>
		/// Devuelve el Grid
		/// </summary>
		/// <value>The grid.</value>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Determina si borra automáticamente la última instancia del <see cref="Moggle.Screens.ScreenThread"/>
		/// </summary>
		public bool TerminateLastScreen { get; set; }

		/// <summary>
		/// Devuelve la pantalla usada para seleccionar
		/// </summary>
		/// <value>The selector screen.</value>
		public SelectTargetScreen SelectorScreen { get; }

		/// <summary>
		/// Devuelve la posición del cursor de selección
		/// </summary>
		/// <value>The current selection point.</value>
		public Point CurrentSelectionPoint
		{
			get{ return SelectorScreen.GridSelector.CursorPosition; }
			set{ SelectorScreen.GridSelector.CursorPosition = value; }
		}

		/// <summary>
		/// La acción que se realizará después de seleccionar.
		/// El argumento es el punto de selección (o <c>null</c> si cancela)
		/// </summary>
		public Action <Point?> Selected { get; }


		/// <summary>
		/// Executa la selección
		/// </summary>
		public void Execute ()
		{
			if (TerminateLastScreen)
				Program.MyGame.ScreenManager.ActiveThread.TerminateLast ();
			
			SelectorScreen.Selected += (sender, e) => Selected?.Invoke (SelectorScreen.GridSelector.CursorPosition);	
			
			SelectorScreen.Execute (ScreenExt.DialogOpt);
		}

		/// <summary>
		/// Ejecuta un selector con los parámetros dados
		/// </summary>
		/// <param name="grid">Grid.</param>
		/// <param name="onSelect">Acción al seleccionar</param>
		/// <param name="startingGridCursor">Posición inicual del cursor</param>
		/// <param name="terminateLast"></param>
		public static void Run (LogicGrid grid,
		                        IUnidad camera,
		                        Action<Point?> onSelect,
		                        Point startingGridCursor,
		                        bool terminateLast = false)
		{
			var newRun = new SelectorController (onSelect, grid, camera);
			newRun.TerminateLastScreen = terminateLast;
			newRun.CurrentSelectionPoint = startingGridCursor;
			newRun.Execute ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Helper.SelectorController"/> class.
		/// </summary>
		/// <param name="onSelect">Acción al seleccionar</param>
		/// <param name="grid">Grid.</param>
		public SelectorController (Action<Point?> onSelect,
		                           LogicGrid grid,
		                           IUnidad camera)
		{
			Grid = grid;
			SelectorScreen = new SelectTargetScreen (Program.MyGame, Grid);
			SelectorScreen.GridSelector.CameraUnidad = camera;
			TerminateLastScreen = false;
			Selected = onSelect;
		}
	}
}