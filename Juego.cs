using System.Diagnostics;


namespace Art_of_Meow
{
	public class Juego : Moggle.Game
	{
		public Juego ()
		{
			Debugger.Launch ();
			Graphics.IsFullScreen = false;
		}
	}
}