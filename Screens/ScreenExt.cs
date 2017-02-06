using Moggle.Screens;

namespace Screens
{
	/// <summary>
	/// Extensiones de Screen
	/// </summary>
	public static class ScreenExt
	{
		/// <summary>
		/// Opciones tipo Dialo
		/// </summary>
		public static readonly ScreenThread.ScreenStackOptions DialogOpt;
		/// <summary>
		/// Opciones default
		/// </summary>
		public static readonly ScreenThread.ScreenStackOptions NewScreen;

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThread thread,
		                            ScreenThread.ScreenStackOptions opt)
		{
			scr.Initialize ();
			scr.LoadContent (scr.Content);
			thread.Stack (scr, opt);
		}

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThreadManager threadMan,
		                            ScreenThread.ScreenStackOptions opt)
		{
			Execute (scr, threadMan.ActiveThread, opt);
		}

		/// <summary>
		/// Ejecuta un screen
		/// </summary>
		public static void Execute (this IScreen scr,
		                            ScreenThread.ScreenStackOptions opt)
		{
			Execute (scr, scr.Juego.ScreenManager, opt);
		}

		static ScreenExt ()
		{
			DialogOpt = new ScreenThread.ScreenStackOptions
			{
				ActualizaBase = false,
				DibujaBase = true
			};
			NewScreen = new ScreenThread.ScreenStackOptions
			{
				ActualizaBase = false,
				DibujaBase = false
			};
		}
	}
}