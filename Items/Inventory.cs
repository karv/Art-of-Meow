using System.Collections.Generic;
using Cells.CellObjects;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle.Screens;
using MonoGame.Extended.BitmapFonts;

namespace Items
{
	/// <summary>
	/// Representa la instancia de un objeto
	/// </summary>
	public interface IItem : IComponent
	{
		string Nombre { get; }

		string DefaultTextureName { get; }

		Color DefaultColor { get; }
	}

	public interface IInventory : IComponent
	{
		ICollection<IItem> Items { get; }
	}

	public class Inventory : IInventory
	{
		public List<IItem> Items { get; }

		ICollection<IItem> IInventory.Items { get { return Items; } }

		public Texture2D IconTexture { get; private set; }

		public BitmapFont Font { get; private set; }

		public string IconTextureName { get; set; }

		public string FontName { get; set; }

		public ContentManager Content { get; }

		public void LoadContent ()
		{
			IconTexture = Content.Load<Texture2D> (IconTextureName);
			Font = Content.Load <BitmapFont> (FontName);
		}

		public void UnloadContent ()
		{
			IconTexture = null;
			Font = null;
		}

		void System.IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		public void Initialize ()
		{
		}

		public IComponentContainerComponent<IGameComponent> Container { get; }

		public Inventory (Screen scr)
		{
			Content = scr.Content;
			Container = scr;
		}
	}

	/// <summary>
	/// Representa un <see cref="IItem"/> en el suelo.
	/// </summary>
	public class GroundItem : GridObject
	{
		public IItem ItemClass { get; }

		public GroundItem (IItem type)
			: base (type.DefaultTextureName)
		{
			ItemClass = type;
		}
	}
}