using System.Collections.Generic;
using System.Linq;
using Items;
using Microsoft.Xna.Framework;
using Moggle.Controles;
using Moggle.Screens;
using Units;
using Units.Equipment;

namespace Screens
{
	public class EquipmentScreen : DialScreen
	{
		public IInventory Inventory;
		public EquipmentManager Equipment;
		List<IEquipment> selectableEquipment;

		public ContenedorSelección<IEquipment> Contenedor { get; }

		public override Color BgColor { get { return Color.DarkGray; } }

		public override void Initialize ()
		{
			base.Initialize ();
			buildEquipmentList ();
		}

		void buildEquipmentList ()
		{
			selectableEquipment = new List<IEquipment> ();
			foreach (var eq in Inventory.Items.OfType<IEquipment> ())
				selectableEquipment.Add (eq);
			foreach (var eq in Equipment.EnumerateEquipment ())
				selectableEquipment.Add (eq);

			foreach (var eq in selectableEquipment)
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


		void Contenedor_cambio_selección (object sender, System.EventArgs e)
		{
			var item = Contenedor.FocusedItem;
			if (Equals (item.Owner, Equipment))
				Equipment.UnequipItem (item);
			else
				Equipment.EquipItem (item);
			rebuildSelection ();
		}

		public EquipmentScreen (IScreen baseScreen, IUnidad unid)
			: base (baseScreen.Juego,
			        baseScreen)
		{
			Inventory = unid.Inventory;
			Equipment = unid.Equipment;
			Contenedor = new ContenedorSelección<IEquipment> (this)
			{
				TextureFondoName = "Interface//win_bg",
				TipoOrden = Contenedor<IEquipment>.TipoOrdenEnum.FilaPrimero,
				TamañoBotón = new MonoGame.Extended.Size (32, 32),
				Posición = new Point (30, 30),
				Márgenes = new MargenType
				{
					Top = 5,
					Left = 5,
					Bot = 5,
					Right = 5
				},
				GridSize = new MonoGame.Extended.Size (15, 15),
				BgColor = Color.LightBlue * 0.5f
			};

			AddComponent (Contenedor);
			Contenedor.Activado += Contenedor_cambio_selección;
		}
	}
}