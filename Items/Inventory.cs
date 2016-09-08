﻿using System.Collections.Generic;
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

		protected void LoadContent ()
		{
			IconTexture = Content.Load<Texture2D> (IconTextureName);
			Font = Content.Load <BitmapFont> (FontName);
		}

		protected void UnloadContent ()
		{
			IconTexture = null;
			Font = null;
		}

		void IComponent.LoadContent ()
		{
			LoadContent ();
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
			LoadContent ();
		}

		public IComponentContainerComponent<IGameComponent> Container { get; }

		public Inventory (Screen scr)
		{
			Content = scr.Content;
			Container = scr;
		}
	}
}