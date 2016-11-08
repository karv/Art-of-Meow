using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units;

namespace Cells.CellObjects
{
	/// <summary>
	/// A genedir grid object
	/// </summary>
	public class GridObject : IGridObject
	{
		/// <summary>
		/// Name of the texture
		/// </summary>
		public readonly string StringTexture;

		/// <summary>
		/// Gets or sets the clor of the texture
		/// </summary>
		/// <value>The color of the use.</value>
		public Color UseColor { get; set; }

		/// <summary>
		/// Gets or sets the depth(draw order)
		/// </summary>
		public float Depth { get; set; }

		/// <summary>
		/// Gets the texture
		/// </summary>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.GridObject"/> class.
		/// </summary>
		/// <param name="texture">Texture name</param>
		/// <param name="grid">Grid</param>
		public GridObject (string texture, Grid grid)
		{
			StringTexture = texture;
			CollidePlayer = false;
			Grid = grid;
		}

		/// <summary>
		/// Desarga el contenido gráfico.
		/// </summary>
		public void UnloadContent ()
		{
			Texture = null;
		}

		/// <summary>
		/// Gets the grid
		/// </summary>
		public Grid Grid { get; }

		/// <summary>
		/// Gets the container of the control.
		///  This could be the Screen or Game itself.
		/// </summary>
		/// <value>The container.</value>
		public IComponentContainerComponent<IControl> Container
		{ get { return Grid as IComponentContainerComponent<IControl>; } }

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public virtual void Initialize ()
		{
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cells.CellObjects.GridObject"/> blocks units' movement
		/// </summary>
		/// <value><c>true</c> if collide player; otherwise, <c>false</c>.</value>
		public bool CollidePlayer { get; set; }

		/// <summary>
		/// Determina si este objeto evita que otro objeto pueda ocupar esta misma celda.
		/// </summary>
		/// <param name="collObj">Coll object.</param>
		public bool Collision (IGridObject collObj)
		{
			return CollidePlayer && collObj is IUnidad;
		}


		/// <summary>
		/// Agrega su textura a la biblioteca
		/// </summary>
		public void AddContent (Moggle.BibliotecaContenido content)
		{
			content.AddContent (StringTexture);
		}

		/// <summary>
		/// Carga la textura
		/// </summary>
		public void InitializeContent (Moggle.BibliotecaContenido content)
		{
			Texture = content.GetContent<Texture2D> (StringTexture);
		}

		/// <summary>
		/// Dibuja el objeto sobre un rectpangulo específico
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="area">Rectángulo de dibujo</param>
		public void Draw (SpriteBatch bat, Rectangle area)
		{
			bat.Draw (
				Texture,
				area, null, UseColor,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depth);
		}

		void IDisposable.Dispose ()
		{
			UnloadContent ();
		}

		public void AddToGrid ()
		{
			Grid.AddCellObject (this);
		}

		public void RemoveFromGrid ()
		{
			Grid.RemoveObject (this);
		}

		/// <summary>
		/// Gets or sets the cell-based localization.
		/// </summary>
		/// <value>The location.</value>
		public Point Location { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.GridObject"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.GridObject"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[GridObject: StringTexture={0}, UseColor={1}, CollidePlayer={2}, Location={3}]",
				StringTexture,
				UseColor,
				CollidePlayer,
				Location);
		}
	}
}