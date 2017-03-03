using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoM;
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
	public class EquipmentScreen : Screen
	{
		/// <summary>
		/// Gets the control that show the item stats
		/// </summary>
		public EtiquetaMultiLínea CursorItemInfo { get; }

		/// <summary>
		/// The inventory of the <see cref="IUnidad"/>
		/// </summary>
		public IInventory Inventory { get { return Unidad.Inventory; } }

		/// <summary>
		/// The equipment manager for the <see cref="IUnidad"/>
		/// </summary>
		public EquipmentManager Equipment { get { return Unidad.Equipment; } }

		HashSet<IItem> selectableEquipment;

		/// <summary>
		/// La unidad del equipment
		/// </summary>
		/// <value>The unidad.</value>
		public IUnidad Unidad { get; }

		/// <summary>
		/// The control that lists the equipment
		/// </summary>
		/// <value>The contenedor.</value>
		public ContenedorSelección<IItem> Contenedor { get; }

		/// <summary>
		/// Color de fondo. <see cref="Color.DarkGray"/>
		/// </summary>
		public override Color? BgColor { get { return Color.DarkGray; } }

		/// <summary>
		/// Inicializa los subcomponentes y actualiza los equipment del usuario
		/// </summary>
		protected override void DoInitialization ()
		{
			base.DoInitialization ();

			// Set contenedor options
			Contenedor.Selection.AllowMultiple = true;
			Contenedor.Selection.AllowEmpty = true;

			buildEquipmentList ();
			rebuildSelection ();
			rebuildCursorItemInfo ();
		}

		void cargarContenido ()
		{
			// Loads items and equipment objects
			foreach (var x in Unidad.Inventory.Items.Union (Unidad.Equipment.EnumerateEquipment ()))
				x.LoadContent (Content);
		}

		/// <summary>
		/// Cargar contenido de cada control incluido.
		/// </summary>
		public override void LoadAllContent ()
		{
			base.LoadAllContent ();
			cargarContenido ();
		}

		void buildEquipmentList ()
		{
			selectableEquipment = new HashSet<IItem> ();
			Contenedor.Clear ();
			foreach (var eq in Inventory.Items.OrderByDescending (z => z is IEquipment))
				selectableEquipment.Add (eq);

			foreach (var eq in selectableEquipment)
				Contenedor.Add (eq);
		}

		void rebuildSelection ()
		{
			Contenedor.Selection.ClearSelection ();
			foreach (var eq in Equipment.EnumerateEquipment ())
				Contenedor.Selection.Select (eq);
		}

		/// <summary>
		/// Rebice señal del teclado.
		/// <c>Esc</c> leaves this screen
		/// </summary>
		public override bool RecibirSeñal (System.Tuple<MonoGame.Extended.InputListeners.KeyboardEventArgs, ScreenThread> data)
		{
			var key = data.Item1;

			if (GlobalKeys.Cancel.Contains (key.Key))
			{
				Juego.ScreenManager.ActiveThread.TerminateLast ();
				return true;
			}			
			return base.RecibirSeñal (data);
		}

		void Contenedor_cambio_selección (object sender, System.EventArgs e)
		{
			var item = Contenedor.FocusedItem;

			var itemEquip = item as IEquipment;
			if (itemEquip != null)
			{
				// Si es equipment, se lo (des)equipa
				if (Equals (itemEquip.Owner, Equipment))
					Equipment.UnequipItem (itemEquip);
				else
					Equipment.EquipItem (itemEquip);
				rebuildSelection ();
				return;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Screens.EquipmentScreen"/> class.
		/// </summary>
		/// <param name="baseScreen">Base screen.</param>
		/// <param name="unid">Unidad</param>
		public EquipmentScreen (IScreen baseScreen, IUnidad unid)
			: base (baseScreen.Juego)
		{
			Unidad = unid;
			Contenedor = new ContenedorSelección<IItem> (this)
			{
				TextureFondoName = "Interface//win_bg",
				TipoOrden = Contenedor<IItem>.TipoOrdenEnum.FilaPrimero,
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
				SelectionEnabled = true,

				DownKey = GlobalKeys.SelectDownKey [0],
				UpKey = GlobalKeys.SelectUp [0],
				LeftKey = GlobalKeys.SelectLeft [0],
				RightKey = GlobalKeys.SelectRight [0],
				EnterKey = GlobalKeys.Accept [0],
			};

			CursorItemInfo = new EtiquetaMultiLínea (this)
			{
				MaxWidth = 300,
				TextColor = Color.WhiteSmoke,
				TopLeft = new Point (30, Contenedor.ControlBounds ().Bottom + 30),
				UseFont = "Fonts//itemfont",
				BackgroundColor = Color.Black
			};

			AddComponent (Contenedor);
			AddComponent (CursorItemInfo);

			Contenedor.Activado += Contenedor_cambio_selección;
			Contenedor.CursorChanged += cursorChanged;
		}

		void rebuildCursorItemInfo ()
		{
			var selEquipment = Contenedor.FocusedItem;
			var tooltipString = new StringBuilder ();
			var fullName = selEquipment.Modifiers.GetName ();
			tooltipString.Append (fullName + "\t\t");
			tooltipString.Append (selEquipment.GetTooltipInfo () + "\t\t");

			foreach (var mod in selEquipment.Modifiers.SquashMods ())
				tooltipString.AppendFormat (
					"{0} : {1}\t",
					mod.AttributeChangeName,
					mod.Delta);

			CursorItemInfo.Texto = tooltipString.ToString ();
		}

		void cursorChanged (object sender, System.EventArgs e)
		{
			rebuildCursorItemInfo ();
		}
	}
}