﻿using System;
using System.Diagnostics;
using AoM;
using Debugging;
using Items.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Items.Declarations
{
	/// <summary>
	/// Common implementation for items
	/// </summary>
	public abstract class CommonItemBase : IItem
	{
		string textureName;

		/// <summary>
		/// Gets or sets the name of the texture
		/// </summary>
		/// <value>The name of the texture.</value>
		public string TextureName
		{
			get
			{
				return textureName;
			}
			set
			{
				if (value == null || IsInitialized)
					throw new InvalidOperationException ();
				textureName = value;
			}
		}

		/// <summary>
		/// Gets the tooltip info (shown in equipment screen)
		/// </summary>
		public virtual string GetTooltipInfo ()
		{
			return string.Empty;
		}

		string IIdentificable.Name
		{ get { return NombreBase; } }

		/// <summary>
		/// Gets the value or worth of the item
		/// </summary>
		public virtual float Value { get { return GetModValue (); } }

		/// <summary>
		/// Gets the calculated value from modifications only
		/// </summary>
		/// <returns>The mod value.</returns>
		public float GetModValue ()
		{
			return Modifiers.ValueSum ();
		}

		Texture2D _texture;

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <remarks>This cannot be set after the object has been initialized, 
		/// si virtually it is a getter </remarks>
		public Texture2D Texture
		{
			get{ return _texture; }
			protected set
			{
				if (IsInitialized)
					throw new InvalidOperationException ("Cannot set texture after initialization.");
				_texture = value;
			}
		}

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the names of the allowed modifications for this item
		/// </summary>
		public string[] AllowedModNames { get; set; }

		ItemModifier[] IItem.AllowedMods
		{
			get
			{
				var ret = new ItemModifier[AllowedModNames.Length];
				for (int i = 0; i < AllowedModNames.Length; i++)
					ret [i] = Program.MyGame.ItemMods [AllowedModNames [i]];
				return ret;
			}
		}

		/// <summary>
		/// Devuelve los modificadores del objeto
		/// </summary>
		[JsonIgnore]
		public ItemModifiersManager Modifiers { get; }

		[JsonProperty ("Modifiers")]
		ItemModifier[] _mods
		{
			get{ return Modifiers.Modifiers.ToArray (); }
		}

		/// <summary>
		/// Determines if this item is initialized
		/// </summary>
		/// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
		[JsonIgnore]
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Loads the texture
		/// </summary>
		protected virtual void LoadContent (ContentManager manager)
		{
			_texture = manager.Load<Texture2D> (TextureName);
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		protected virtual void Initialize ()
		{
			Debug.WriteLineIf (
				IsInitialized,
				string.Format ("Object {0} initialized multiple times.", this),
				DebugCategories.Item);
			IsInitialized = true;
		}

		/// <summary>
		/// Dibuja el objeto sobre un rectángulo específico
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="rect">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Texture, rect, Color);
		}

		#region IComponent implementation

		void Moggle.Controles.IComponent.LoadContent (ContentManager manager)
		{
			LoadContent (manager);
		}

		#endregion

		#region IGameComponent implementation

		void IGameComponent.Initialize ()
		{
			Initialize ();
		}

		#endregion

		#region IItem implementation

		/// <summary>
		/// Gets the name of the item
		/// </summary>
		/// <value>The nombre.</value>
		public string NombreBase { get; }

		string IItem.DefaultTextureName { get { return TextureName; } }

		Color IItem.DefaultColor { get { return Color; } }

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Items.Declarations.CommonItemBase"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Items.Declarations.CommonItemBase"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("[CommonItemBase: {0}]", NombreBase);
		}

		/// <summary>
		/// Clones this item, without owner or modifications
		/// </summary>
		public abstract object Clone ();

		/// <summary>
		/// Initializes a new instance of the <see cref="Items.Declarations.CommonItemBase"/> class.
		/// </summary>
		/// <param name="nombre">Nombre.</param>
		/// <param name = "allowedModNames">The array with the names of allowed 
		/// modification for this item. null is passed as as empty array</param>
		protected CommonItemBase (string nombre, string [] allowedModNames = null)
		{
			NombreBase = nombre;
			Modifiers = new ItemModifiersManager (this);
			AllowedModNames = allowedModNames ?? new string[]{ };
		}
	}
}