using AoM;
using Cells;
using Microsoft.Xna.Framework;
using Screens;
using System;

namespace Helper
{
	public sealed class SelectorController
	{
		public void Execute (LogicGrid grid, bool terminateLast = false)
		{
			var executionScreen = new SelectTargetScreen (Program.MyGame, grid);

			if (terminateLast)
				Program.MyGame.ScreenManager.ActiveThread.TerminateLast ();
			
			executionScreen.Selected += (sender, e) => Selected?.Invoke (executionScreen.GridSelector.CursorPosition);	
			
			executionScreen.Execute (ScreenExt.DialogOpt);
		}

		public Action <Point?> Selected { get; }

		public static void Run (LogicGrid grid,
		                        Action<Point?> onSelect,
		                        bool terminateLast = false)
		{
			var newRun = new SelectorController (onSelect);
			newRun.Execute (grid, terminateLast);
		}

		public SelectorController (Action<Point?> onSelect)
		{
			Selected = onSelect;
		}
	}
	
}