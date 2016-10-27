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
	/// <summary>
	/// This screen allows the user to change the equipment of a <see cref="IUnidad"/>
	/// </summary>
	public class EquipmentScreen : DialScreen
	{
		/// <summary>
		/// The inventory of the <see cref="IUnidad"/>
		/// </summary>
		public IInventory Inventory;
		/// <summary>
		/// The equipment manager for the <see cref="IUnidad"/>
		/// </summary>
		public EquipmentManager Equipment;
		List<IEquipment> selectableEquipment;

		/// <summary>
		/// The control that lists the equipment
		/// </summary>
		/// <value>The contenedor.</value>
		public ContenedorSelección<IEquipment> Contenedor { get; }

		/// <summary>
		/// Color de fondo. <see cref="Color.DarkGray"/>
		/// </summary>
		public override Color BgColor { get { return Color.DarkGray; } }

		/// <summary>
		/// Initializes its controls,
		/// reloads the equipment list,
		/// marks the equiped items
		/// </summary>
		public override void Initialize ()
		{
			base.Initialize ();
			buildEquipmentList ();
			rebuildSelection ();
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

		/// <summary>
		/// Always draw screen base
		/// </summary>
		public override bool DibujarBase{ get { return true; } }

		/// <summary>
		/// Rebice señal del teclado.
		/// <c>Esc</c> leaves this screen
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.EquipmentScreen"/> class.
		/// </summary>
		/// <param name="baseScreen">Base screen.</param>
		/// <param name="unid">Unidad</param>
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
				MargenExterno = new MargenType
				{
					Top = 5,
					Left = 5,
					Bot = 5,
					Right = 5
				},
				MargenInterno = new MargenType
				{
					Top = 1,
					Left = 1,
					Bot = 1,
					Right = 1
				},
				GridSize = new MonoGame.Extended.Size (15, 15),
				BgColor = Color.LightBlue * 0.5f,
				SelectionEnabled = true
			};

			AddComponent (Contenedor);
			Contenedor.Activado += Contenedor_cambio_selección;
		}
	}
}