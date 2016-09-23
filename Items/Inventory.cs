using System.Collections.Generic;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle.Screens;
using MonoGame.Extended.BitmapFonts;
using System.Linq;
using System;

namespace Items
{
	public class Inventory : IInventory
	{
		public List<IItem> Items { get; }

		ICollection<IItem> IInventory.Items { get { return Items; } }

		public Texture2D IconTexture { get; private set; }

		public BitmapFont Font { get; private set; }

		public string IconTextureName { get; set; }

		public string FontName { get; set; }

		public IEnumerable<T> ItemsOfType<T> ()
		{
			return Items.OfType<T> ();
		}

		public IEnumerable<T> ItemsOfType<T> (Func<T, bool> pred)
		{
			return Items.OfType<T> ().Where (pred);
		}

		public IEnumerable<IItem> Where (Func<IItem, bool> pred)
		{
			return Items.Where (pred);
		}

		protected void LoadContent (ContentManager manager)
		{
			IconTexture = manager.Load<Texture2D> (IconTextureName);
			Font = manager.Load <BitmapFont> (FontName);
		}

		protected void UnloadContent ()
		{
			IconTexture = null;
			Font = null;
		}

		void IComponent.LoadContent (ContentManager manager)
		{
			LoadContent (manager);
		}

		void IComponent.UnloadContent ()
		{
			UnloadContent ();
		}

		void System.IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		public virtual void Initialize ()
		{
		}

		public ILookup<string, IItem> Group ()
		{
			return Items.ToLookup (i => i.Nombre);
		}

		public Inventory ()
		{
			Items = new List<IItem> ();
		}
	}
}