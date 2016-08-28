using Cells.CellObjects;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Cells;

namespace Units
{
	public class UnidadHumano : IUnidad
	{
		public ManejadorRecursos Recursos { get; }

		public Grid MapGrid { get; set; }

		public const string TextureType = "person";

		public string TextureStr { get; protected set; }

		public Texture2D Texture { get; protected set; }

		public void LoadContent (ContentManager content)
		{
			Texture = content.Load<Texture2D> (TextureStr);
		}

		public bool Collision (IGridObject collObj)
		{
			return collObj is UnidadHumano; 
		}

		public void Draw (Rectangle area, SpriteBatch bat)
		{
			bat.Draw (
				Texture,
				area, null, Color.White,
				0, Vector2.Zero,
				SpriteEffects.None,
				Depths.Unidad);
		}

		public void Dispose ()
		{
			Texture = null;
		}

		public Point Location { get; set; }

		public void MeleeDamage (IUnidad target)
		{
			target.Die ();
		}

		/// <summary>
		/// Move or melee to a direction
		/// </summary>
		/// <returns><c>true</c>, if action was taken, <c>false</c> otherwise.</returns>
		/// <param name="dir">Direction</param>
		public bool MoveOrMelee (MovementDirectionEnum dir)
		{
			if (!MapGrid.MoveCellObject (this, dir))
			{
				var targetCell = new Cell (MapGrid, Location + dir.AsDirectionalPoint ());
				var target = targetCell.GetUnidadHere ();
				if (target == null)
					return false;
				MeleeDamage (target);
			}
			return true;
		}

		public UnidadHumano (string texture = TextureType)
		{
			TextureStr = texture;
			Recursos = new ManejadorRecursos ();
		}
	}
}