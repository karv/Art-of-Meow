using System.IO;
using Items;
using Items.Modifiers;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Textures;
using Units;

namespace AoM
{
	/// <summary>
	/// The game engine
	/// </summary>
	public class Juego : Moggle.Game
	{
		#region Texturas definidas

		/// <summary>
		/// Gets the set of predefined run-time-generated <see cref="Texture"/>
		/// </summary>
		/// <value>The textures.</value>
		public static Textures Textures { get; private set; }

		#endregion

		/// <summary>
		/// Gets the item modification database
		/// </summary>
		public ItemModifierDatabase ItemMods { get; private set; }

		/// <summary>
		/// The database for item types
		/// </summary>
		/// <value>The items.</value>
		public ItemDatabase Items { get; set; }

		/// <summary>
		/// Gets the class and race manager
		/// </summary>
		public UnitClassRaceManager ClassRaceManager { get; set; }

		/// <summary>
		/// Carga el contenido del juego, incluyendo los controles universales.
		/// </summary>
		protected override void LoadContent ()
		{
			try
			{
				base.LoadContent ();
			}
			catch (System.Exception ex)
			{
				System.Console.WriteLine ("Critical error: cannot load content");
				throw ex;
			}
			Textures = new Textures (GraphicsDevice);
		}

		/// <summary>
		/// </summary>
		protected override void Initialize ()
		{
			SimpleTextureGenerator = new SimpleTextures (GraphicsDevice);

			// Load files
			LoadDBContent ();

			base.Initialize ();

		}

		void LoadDBContent ()
		{
			var file = File.OpenText (FileNames.ItemModifiers);
			var jsonStr = file.ReadToEnd ();
			file.Close ();

			ItemMods = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemModifierDatabase> (
				jsonStr,
				Map.JsonSets);

			Items = ItemDatabase.FromFile ();

			if (ItemMods == null)
				throw new IOException ("Cannot load ItemMods database");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AoM.Juego"/> class.
		/// </summary>
		public Juego ()
		{
			Content.RootDirectory = @"Content/Content";
			Graphics.IsFullScreen = true;
		}

		/// <summary>
		/// Commor generator for simple definible textures
		/// </summary>
		public SimpleTextures SimpleTextureGenerator { get; private set; }
	}

	/// <summary>
	/// Set of easy definable textures
	/// </summary>
	public class Textures
	{
		/// <summary>
		/// Solid white color texture
		/// </summary>
		public readonly Texture2D SolidTexture;

		/// <summary>
		/// Initializes a new instance of the <see cref="AoM.Textures"/> class.
		/// </summary>
		/// <param name="gd">Gd.</param>
		internal Textures (GraphicsDevice gd)
		{
			SolidTexture = new Texture2D (gd, 1, 1);
			var data = new Color[1];
			data [0] = Color.White;
			SolidTexture.SetData (data);
		}
	}
}