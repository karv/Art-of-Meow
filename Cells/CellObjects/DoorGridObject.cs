using System;
using Cells.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Moggle.Controles;

namespace Cells.CellObjects
{
	/// <summary>
	/// It's a door
	/// </summary>
	public class DoorGridObject : ICollidableGridObject, IActivable
	{
		/// <summary>
		/// Devuelve el Grid
		/// </summary>
		/// <value>The grid.</value>
		public LogicGrid Grid { get; }

		/// <summary>
		/// Opens the door
		/// </summary>
		public void Open ()
		{
			IsOpen = true;
		}

		bool IGridObject.BlockVisibility { get { return !IsOpen; } }

		void IActivable.Activar ()
		{
			Open ();
			AlActivar?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets the current texture
		/// </summary>
		public Texture2D CurrentTexture 
		{ get { return IsOpen ? TextureOpen : TextureClosed; } }

		Texture2D IGridObject.Texture { get { return CurrentTexture; } }

		/// <summary>
		/// Gets or sets a value indicating whether this door is open (or closed)
		/// </summary>
		/// <value><c>true</c> if this door is open; otherwise, <c>false</c>.</value>
		public bool IsOpen { get; set; }

		// "open-door";
		const string StringTextureOpen = "open door";
		//"closed-door";
		const string StringTextureClosed = "closed door";

		/// <summary>
		/// La profundidad de dibujo.
		/// </summary>
		public float Depth { get { return Depths.Foreground; } }

		/// <summary>
		/// La textura de dibujo
		/// </summary>
		public Texture2D TextureOpen { get; private set; }

		/// <summary>
		/// La textura de dibujo
		/// </summary>
		public Texture2D TextureClosed { get; private set; }

		System.Collections.Generic.IEnumerable<ICollisionRule> ICollidableGridObject.GetCollisionRules ()
		{
			yield return new DescriptCollitionRule (z => !IsOpen);
		}

		void IDibujable.Draw (SpriteBatch bat, Rectangle rect)
		{
			bat.Draw (CurrentTexture, destinationRectangle: rect, layerDepth: Depth);
		}

		void IComponent.LoadContent (ContentManager manager)
		{
			TextureOpen = manager.Load<Texture2D> (StringTextureOpen);
			TextureClosed = manager.Load<Texture2D> (StringTextureClosed);
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
			return string.Format ("{1} door@{0}", Location, IsOpen ? "Open" : "Closed");
		}

		/// <summary>
		/// Gets the cell-based localization.
		/// </summary>
		/// <value>The location.</value>
		public Point Location { get; set; }

		IComponentContainerComponent<IControl> IControl.Container
		{ get { return Grid as IComponentContainerComponent<IControl>; } }

		/// <summary>
		/// Ocurre cuando se activa.
		/// </summary>
		public event EventHandler AlActivar;

		/// <summary>
		/// Initializes a new instance of the <see cref="Cells.CellObjects.GridWall"/> class.
		/// </summary>
		public DoorGridObject (LogicGrid grid)
		{
			Grid = grid;
		}
	}
}