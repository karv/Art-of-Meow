using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Cells;

namespace Items.Declarations
{
	public abstract class CommonItemBase : IItem
	{
		public abstract string Nombre { get; }

		public abstract string TextureName { get; }

		public Texture2D Texture { get; protected set; }

		public Color Color { get { return GetColor (); } }

		public ContentManager Content { get { return GridMap.Screen.Content; } }

		public Grid GridMap { get; }

		protected abstract Color GetColor ();

		protected virtual void LoadContent ()
		{
			Texture = Content.Load<Texture2D> (TextureName);
		}

		protected virtual void UnloadContent ()
		{
			Texture = null;
		}

		protected virtual void Initialize ()
		{
		}

		#region IComponent implementation

		void Moggle.Controles.IComponent.LoadContent ()
		{
			LoadContent ();
		}

		void Moggle.Controles.IComponent.UnloadContent ()
		{
			UnloadContent ();
		}

		Moggle.Controles.IComponentContainerComponent<IGameComponent> Moggle.Controles.IComponent.Container
		{ get { return (Moggle.Controles.IComponentContainerComponent<IGameComponent>)GridMap; } }

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

		protected CommonItemBase (Grid grid)
		{
			GridMap = grid;
		}
	}
}