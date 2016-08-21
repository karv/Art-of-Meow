using Moggle.Screens;
using Microsoft.Xna.Framework;

namespace Art_of_Meow
{

	public class SomeScreen : Screen
	{
		public override Color BgColor
		{
			get
			{
				return Color.Blue;
			}
		}

		public SomeScreen (Moggle.Game game)
			: base (game)
		{
		}
		
	}
	
}