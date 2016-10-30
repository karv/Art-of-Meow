using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using MonoGame.Extended.BitmapFonts;

namespace Items
{
	/// <summary>
	/// Inventory of a <see cref="Units.IUnidad"/>
	/// </summary>
	public class Inventory : IInventory
	{
		/// <summary>
		/// The list of items
		/// </summary>
		/// <value>The items.</value>
		public List<IItem> Items { get; }

		ICollection<IItem> IInventory.Items { get { return Items; } }

		//  THINK: ¿Qué es esto?
		/// <summary>
		/// </summary>
		public Texture2D IconTexture { get; private set; }
		//  THINK: ¿Qué es esto?
		/// <summary>
		/// </summary>
		public string IconTextureName { get; set; }

		/// <summary>
		/// Font to use whe listing items
		/// </summary>
		/// <value>The font.</value>
		public BitmapFont Font { get; private set; }

		/// <summary>
		/// Gets or sets the name of the font
		/// </summary>
		/// <value>The name of the font.</value>
		public string FontName { get; set; }

		/// <summary>
		/// Enumerates the items of a given type
		/// </summary>
		/// <typeparam name="T">Type of items to filter</typeparam>
		public IEnumerable<T> ItemsOfType<T> ()
		{
			return Items.OfType<T> ();
		}

		/// <summary>
		/// Enmerates the items of a given type satisfacing a predicate
		/// </summary>
		/// <returns>The of type.</returns>
		/// <param name="pred">Predicate</param>
		/// <typeparam name="T">Type of items</typeparam>
		public IEnumerable<T> ItemsOfType<T> (Func<T, bool> pred)
		{
			return Items.OfType<T> ().Where (pred);
		}

		/// <summary>
		/// Selects a subset of items
		/// </summary>
		public IEnumerable<IItem> Where (Func<IItem, bool> pred)
		{
			return Items.Where (pred);
		}

		/// <summary>
		/// Carga el contenido gráfico.
		/// </summary>
		/// <param name="manager">Manager.</param>
		public void LoadContent (ContentManager manager)
		{
			// TODO: eventualmente debería tener algo de texto

			//IconTexture = manager.Load<Texture2D> (IconTextureName);
			//Font = manager.Load <BitmapFont> (FontName);

			foreach (var item in Items)
				item.LoadContent (manager);
		}

		/// <summary>
		/// Desarga el contenido gráfico.
		/// </summary>
		public void UnloadContent ()
		{
			IconTexture = null;
			Font = null;
		}

		/// <summary>
		/// Determines if theres any item in the inventory
		/// </summary>
		public bool Any ()
		{
			return Items.Any ();
		}

		/// <summary>
		/// Adds an item
		/// </summary>
		/// <param name="item">Item.</param>
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

		void IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public virtual void Initialize ()
		{
		}

		/// <summary>
		/// Groups the items by type
		/// </summary>
		public ILookup<string, IItem> Group ()
		{
			return Items.ToLookup (i => i.Nombre);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Items.Inventory"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Items.Inventory"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[Inventory:\n{0}]", Items);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Inventory"/> class.
		/// </summary>
		public Inventory ()
		{
			Items = new List<IItem> ();
		}
	}
}