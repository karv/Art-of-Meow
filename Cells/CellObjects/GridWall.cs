using System;
using Cells.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;
using Moggle;

namespace Cells.CellObjects
{
	/// <summary>
	/// Representa un muro de Grid
	/// </summary>
	public class GridWall : ICollidableGridObject
	{
		/// <summary>
		/// Devuelve el Grid
		/// </summary>
		/// <value>The grid.</value>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Nombre de la textura
		/// </summary>
		public readonly string StringTexture;

		protected BibliotecaContenido Content { get; }

		/// <summary>
		/// La profundidad de dibujo.
		/// </summary>
		public float Depth { get { return Depths.Foreground; } }

		/// <summary>
		/// La textura de dibujo
		/// </summary>
		public Texture2D Texture { get; private set; }

		System.Collections.Generic.IEnumerable<ICollisionRule> ICollidableGridObject.GetCollisionRules ()
		{
			yield return new DescriptCollitionRule (z => true);
		}

		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (Texture, destinationRectangle: rect, layerDepth: Depth);
		}

		void IComponent.AddContent ()
		{
			Content.AddContent (StringTexture);
		}

		void IComponent.InitializeContent ()
		{
			Texture = Content.GetContent<Texture2D> (StringTexture);
		}

		void IDisposable.Dispose ()
		{
		}

		void IGameComponent.Initialize ()
		{
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.GridWall"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Cells.CellObjects.GridWall"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("Wall@{0}", Location);
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		/// <value>The location.</value>
		public Point Location { get; set; }

		IComponentContainerComponent<IControl> IControl.Container
		{ get { return Grid as IComponentContainerComponent<IControl>; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.GridWall"/> class.
		/// </summary>
		/// <param name="stringTexture">Name of the texture</param>
		/// <param name="grid">Grid.</param>
		public GridWall (string stringTexture,
		                 LogicGrid grid,
		                 BibliotecaContenido content)
		{
			StringTexture = stringTexture;
			Grid = grid;
			Content = content;
		}
	}
}