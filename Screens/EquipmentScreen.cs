using System.Linq;
using Items;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using System;
using Units.Equipment;

namespace Screens
{
	public class EquipmentScreen : DialScreen
	{
		public Inventory Inventory;
		public EquipmentManager Equipment;

		public ContenedorSelección<IEquipment> Contenedor { get; }

		public override Color BgColor { get { return Color.Gray * 0.8f; } }

		public override void Initialize ()
		{
			base.Initialize ();
			buildEquipmentList ();
		}

		void buildEquipmentList ()
		{
			foreach (var eq in Inventory.Items.OfType<IEquipment> ())
				Contenedor.Add (eq);

			Contenedor.Selection.AllowMultiple = true;
			Contenedor.Selection.AllowEmpty = true;
		}

		void rebuildSelection ()
		{
			Contenedor.Selection.ClearSelection ();
			foreach (var eq in Equipment.EnumerateEquipment ())
				Contenedor.Selection.Select (eq);
			
		}

		public override bool DibujarBase{ get { return true; } }

		public override bool RecibirSeñal (MonoGame.Extended.InputListeners.KeyboardEventArgs key)
		{
			if (key.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
				Salir ();
			return base.RecibirSeñal (key);
		}

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