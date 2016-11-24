using System.Linq;
using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Screens;
using System;
using Skills;
using System.Collections.Generic;
using Units;
using Moggle.Screens.Dials;

namespace Helper
{
	/// <summary>
	/// Provee métodos para ayudar en la selección de un punto en un <see cref="LogicGrid"/>
	/// </summary>
	[Obsolete]
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

		public Cell SelectedCell ()
		{
			return Grid.GetCell (CurrentSelectionPoint);
		}

		/*
		/// <summary>
		/// La acción que se realizará después de seleccionar.
		/// El argumento es el punto de selección (o <c>null</c> si cancela)
		/// </summary>
		public Action <Point?> Selected { get; }
		*/

		public Predicate<ITarget> AcceptingTargets = z => true;
		public Func<ITarget, IEffect []> EffectMaker;

		protected SkillInstance Instance { get; }

		public bool IsResponse { get; protected set; }

		/// <summary>
		/// Executa la selección
		/// </summary>
		public void Execute ()
		{
			if (TerminateLastScreen)
				Program.MyGame.ScreenManager.ActiveThread.TerminateLast ();
			
			SelectorScreen.HayRespuesta += delegate
			{
				IsResponse = true;
				Response?.Invoke (this, EventArgs.Empty);
			};
			
			SelectorScreen.Execute (ScreenExt.DialogOpt);
		}

		List<IEffect> _currentEffects = new List<IEffect> ();

		void rebuildEffects ()
		{
			_currentEffects.Clear ();
			var selCell = SelectedCell ();
			foreach (var c in selCell.EnumerateObjects ().Where (z => AcceptingTargets(z)))
			{
				var newEffect = EffectMaker (c);
				foreach (var ef in newEffect)
					_currentEffects.Add (ef);
			}
		}

		public event EventHandler Response;

		/// <summary>
		/// Ejecuta un selector con los parámetros dados
		/// </summary>
		/// <param name="grid">Grid.</param>
		/// <param name = "camera"></param>
		/// <param name="startingGridCursor">Posición inicual del cursor</param>
		/// <param name="terminateLast"></param>
		[Obsolete]
		public static void Run (LogicGrid grid,
		                        IUnidad camera,
		                        Point startingGridCursor,
		                        bool terminateLast = false)
		{
			var newRun = new SelectorController (grid, camera);
			newRun.TerminateLastScreen = terminateLast;
			newRun.CurrentSelectionPoint = startingGridCursor;
			newRun.Execute ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Helper.SelectorController"/> class.
		/// </summary>
		/// <param name="camera">Unidad que persigue la cámara</param>
		/// <param name="grid">Grid.</param>
		public SelectorController (LogicGrid grid, IUnidad camera)
		{
			Grid = grid;
			SelectorScreen = new SelectTargetScreen (Program.MyGame, Grid);
			SelectorScreen.GridSelector.CameraUnidad = camera;
			TerminateLastScreen = false;
		}
	}
}