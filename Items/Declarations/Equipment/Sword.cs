using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : IEquipment
	{
		public string IconString { get; }

		public Texture Icon { get; protected set; }

		#region IComponent implementation

		public void LoadContent (ContentManager manager)
		{
			Icon = manager.Load<Texture2D> (IconString);
		}

		public void UnloadContent ()
		{
			Debug.WriteLineIf (
				Owner == null,
				"Disposing equiped item " + this,
				"Equipment");
			Owner?.UnequipItem (this);
		}

		public Moggle.Controles.IComponentContainerComponent<IGameComponent> Container
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IDisposable implementation

		void IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
		}

		#endregion

		#region IEquipment implementation

		public EquipSlot Slot
		{
			get { return EquipSlot.None; }
		}

		public Units.Equipment.EquipmentManager Owner { get; set; }

		#endregion

		#region IItem implementation

		public string Nombre { get; }

		public string DefaultTextureName
		{
			get { return IconString; }
		}

		public Color DefaultColor
		{
			get { return Color.Transparent; }
		}

		#endregion

		protected Sword (string icon)
		{
			IconString = icon;
		}

		public Sword ()
			: this ("sword")
		{
		}
	}
}