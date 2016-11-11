using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Units;
using Moggle;

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

		protected BibliotecaContenido Content { get; }

		/// <summary>
		/// Gets or sets the depth(draw order)
		/// </summary>
		public float Depth { get; set; }

		/// <summary>
		/// Gets the texture
		/// </summary>
		public Texture2D Texture { get; private set; }


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
		public LogicGrid Grid { get; }

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
		public void AddContent ()
		{
			Content.AddContent (StringTexture);
		}

		/// <summary>
		/// Carga la textura
		/// </summary>
		public void InitializeContent ()
		{
			Texture = Content.GetContent<Texture2D> (StringTexture);
		}

		/// <summary>
		/// Dibuja el objeto sobre un rectpangulo específico
		/// </summary>
		/// <param name="bat">Batch</param>
		/// <param name="area">Rectángulo de dibujo</param>
		public virtual void Draw (SpriteBatch bat, Rectangle area)
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
			Dispose ();
		}

		/// <summary>
		/// Descarga el contenido gráfico
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Cells.CellObjects.GridObject"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Cells.CellObjects.GridObject"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the <see cref="Cells.CellObjects.GridObject"/>
		/// so the garbage collector can reclaim the memory that the <see cref="Cells.CellObjects.GridObject"/> was occupying.</remarks>
		protected virtual void Dispose ()
		{
			UnloadContent ();
		}

		/// <summary>
		/// Agrega este objeto al <see cref="Grid"/>
		/// </summary>
		public void AddToGrid ()
		{
			Grid.AddCellObject (this);
		}

		/// <summary>
		/// Elimina este objeto del <see cref="Grid"/>
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.GridObject"/> class.
		/// </summary>
		/// <param name="texture">Texture name</param>
		/// <param name="grid">Grid</param>
		public GridObject (string texture,
		                   LogicGrid grid,
		                   BibliotecaContenido content)
		{
			StringTexture = texture;
			CollidePlayer = false;
			Grid = grid;
			Content = content;
		}
	}
}