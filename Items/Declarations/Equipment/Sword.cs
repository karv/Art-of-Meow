using System;
using Microsoft.Xna.Framework.Content;

namespace Items.Declarations.Equipment
{
	/// <summary>
	/// Una espada sin propiedades especiales
	/// </summary>
	public class Sword : IEquipment
	{
		public string TextureString { get; }

		#region IComponent implementation

		public void LoadContent (ContentManager manager)
		{
			throw new NotImplementedException ();
		}

		public void UnloadContent ()
		{
			throw new NotImplementedException ();
		}

		public Moggle.Controles.IComponentContainerComponent<Microsoft.Xna.Framework.IGameComponent> Container
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IGameComponent implementation

		public void Initialize ()
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IEquipment implementation

		public EquipSlot Slot
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public Units.Equipment.EquipmentManager Owner
		{
			get
			{
				throw new NotImplementedException ();
			}
			set
			{
				throw new NotImplementedException ();
			}
		}

		#endregion

		#region IItem implementation

		public string Nombre
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public string DefaultTextureName
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public Microsoft.Xna.Framework.Color DefaultColor
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		#endregion



		public Sword ()
		{
		}
	}
}

