using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cells.CellObjects
{
	/// <summary>
	/// Un objeto de <see cref="Grid"/> que no interactúa
	/// </summary>
	public class BackgroundObject : IGridObject
	{
		/// <summary>
		/// The name of the texture
		/// </summary>
		public readonly string StringTexture;

		float IGridObject.Depth
		{
			get { return Depths.BackgroundGridObject; }
		}

		/// <summary>
		/// Gets the texture
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// Desarga el contenido gráfico.
		/// </summary>
		public void UnloadContent ()
		{
		}

		bool IGridObject.BlockVisibility
		{
			get			{ return false; }
		}

		/// <summary>
		/// Gets the grid.
		/// </summary>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Gets the container of the control.
		///  This could be the Screen or Game itself.
		/// </summary>
		/// <value>The container.</value>
		public Moggle.Controles.IComponentContainerComponent<Moggle.Controles.IControl> Container { get; }

		/// <summary>
		/// Loads the content using a given manager
		/// </summary>
		public void LoadContent (Microsoft.Xna.Framework.Content.ContentManager manager)
		{
			Texture = manager.Load<Texture2D> (StringTexture);
		}

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
		}

		/// <summary>
		/// Constante de color
		/// </summary>
		static Color useColor { get { return Color.White; } }

		/// <summary>
		/// Dibuja el objeto sobre un rectpangulo específico
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="area">Area.</param>
		public void Draw (SpriteBatch bat, Rectangle area)
		{
			bat.Draw (
				Texture,
				area, null, useColor,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.BackgroundGridObject);
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		/// <value>The location.</value>
		public Point Location { get; set; }

		void IDisposable.Dispose ()
		{
			Texture = null;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.BackgroundObject"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.BackgroundObject"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[BackgroundObject: StringTexture={0}, Location={1}]",
				StringTexture,
				Location);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.BackgroundObject"/> class.
		/// </summary>
		/// <param name="loc">Location.</param>
		/// <param name="texture">Texture.</param>
		/// <param name="grid">Grid.</param>
		public BackgroundObject (Point loc,
		                         string texture, 
		                         LogicGrid grid)
		{
			StringTexture = texture;
			Location = loc;
			Grid = grid;
		}
	}
}