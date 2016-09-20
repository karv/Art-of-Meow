using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Items.Declarations
{
	public abstract class CommonItemBase : IItem
	{
		public abstract string Nombre { get; }

		public abstract string TextureName { get; }

		public Texture2D Texture { get; protected set; }

		public Color Color { get { return GetColor (); } }

		protected abstract Color GetColor ();

		protected virtual void LoadContent (ContentManager manager)
		{
			Texture = manager.Load<Texture2D> (TextureName);
		}

		protected virtual void UnloadContent ()
		{
		}

		protected virtual void Initialize ()
		{
		}

		#region IComponent implementation

		void Moggle.Controles.IComponent.LoadContent (ContentManager manager)
		{
			LoadContent (manager);
		}

		void Moggle.Controles.IComponent.UnloadContent ()
		{
			UnloadContent ();
		}

		Moggle.Controles.IComponentContainerComponent<IGameComponent> Moggle.Controles.IComponent.Container
		{ get { return null; } }

		#endregion

		#region IDisposable implementation

		void IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
			Initialize ();
		}

		#endregion

		#region IItem implementation

		string IItem.Nombre
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		string IItem.DefaultTextureName
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		Color IItem.DefaultColor
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		#endregion

		protected CommonItemBase ()
		{
		}
	}
}