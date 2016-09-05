using System.Collections.Generic;
using Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle.Screens;
using MonoGame.Extended.BitmapFonts;

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
}