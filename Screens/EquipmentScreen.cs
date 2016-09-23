using Moggle.Screens;

namespace Screens
{
	public class EquipmentScreen : DialScreen
	{
		public EquipmentScreen (IScreen baseScreen)
			: base (baseScreen.Juego,
			        baseScreen)
		{
		}
	}
}