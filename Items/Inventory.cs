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

		public void LoadContent (ContentManager manager)
		{
			// TODO: eventualmente debería tener algo de texto

			//IconTexture = manager.Load<Texture2D> (IconTextureName);
			//Font = manager.Load <BitmapFont> (FontName);

			foreach (var item in Items)
				item.LoadContent (manager);
		}

		public void UnloadContent ()
		{
			IconTexture = null;
			Font = null;
		}

		public bool Any ()
		{
			return Items.Any ();
		}

		public void Add (IItem item)
		{
			Items.Add (item);
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

		public override string ToString ()
		{
			return string.Format ("[Inventory:\n{0}]", Items);
		}

		public Inventory ()
		{
			Items = new List<IItem> ();
		}
	}
}