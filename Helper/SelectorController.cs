using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Screens;
using System;

namespace Helper
{
	public sealed class SelectorController
	{
		public LogicGrid Grid { get; }

		public bool TerminateLastScreen { get; set; }

		public SelectTargetScreen SelectorScreen { get; }

		public Point CurrentSelectionPoint
		{
			get{ return SelectorScreen.GridSelector.CursorPosition; }
			set{ SelectorScreen.GridSelector.CursorPosition = value; }
		}


		public void Execute ()
		{
			if (TerminateLastScreen)
				Program.MyGame.ScreenManager.ActiveThread.TerminateLast ();
			
			SelectorScreen.Selected += (sender, e) => Selected?.Invoke (SelectorScreen.GridSelector.CursorPosition);	
			
			SelectorScreen.Execute (ScreenExt.DialogOpt);
		}

		public Action <Point?> Selected { get; }

		public static void Run (LogicGrid grid,
		                        Action<Point?> onSelect,
		                        Point startingGridCursor,
		                        bool terminateLast = false)
		{
			var newRun = new SelectorController (onSelect, grid);
			newRun.TerminateLastScreen = terminateLast;
			newRun.CurrentSelectionPoint = startingGridCursor;
			newRun.Execute ();
		}

		public SelectorController (Action<Point?> onSelect, LogicGrid grid)
		{
			Grid = grid;
			SelectorScreen = new SelectTargetScreen (Program.MyGame, Grid);
			TerminateLastScreen = false;
			Selected = onSelect;
		}
	}
	
}