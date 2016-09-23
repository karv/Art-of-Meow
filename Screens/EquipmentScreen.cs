using System.Linq;
using Items;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;

namespace Screens
{
	public class EquipmentScreen : DialScreen
	{
		public Inventory Inventory;

		public ContenedorSelección<IEquipment> Contenedor { get; }

		public override void Initialize ()
		{
			base.Initialize ();
			buildEquipmentList ();
		}

		void buildEquipmentList ()
		{
			foreach (var eq in Inventory.Items.OfType<IEquipment> ())
				Contenedor.Add (eq);

			Contenedor.Selection.AllowMultiple = false;
			Contenedor.Selection.AllowEmpty = false;

		}

		public override bool DibujarBase{ get { return true; } }

		public EquipmentScreen (IScreen baseScreen, Inventory inv)
			: base (baseScreen.Juego,
			        baseScreen)
		{
			Inventory = inv;
			Contenedor = new ContenedorSelección<IEquipment> (this)
			{
				TextureFondoName = "brick-wall",
				TipoOrden = Contenedor<IEquipment>.TipoOrdenEnum.FilaPrimero,
				TamañoBotón = new MonoGame.Extended.Size (16, 16),
				Posición = new Point (30, 30),
				Márgenes = new MargenType
				{
					Top = 5,
					Left = 5,
					Bot = 5,
					Right = 5
				},
				GridSize = new MonoGame.Extended.Size (30, 30),
				BgColor = Color.Black
			};

			AddComponent (Contenedor);
		}
	}
}